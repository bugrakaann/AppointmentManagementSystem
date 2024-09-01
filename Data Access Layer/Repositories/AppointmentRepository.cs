using Data_Access_Layer.Data;
using Data_Access_Layer.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;
using Models.Enums;
using Models.Models;

namespace Data_Access_Layer.Repositories;

public class AppointmentRepository : Repository<Appointment>, IAppointmentRepository
{
    private readonly DbContext _context;
    private readonly DbSet<Appointment> _dbSet;

    public AppointmentStatus[] ValidStatuses { get; set; }

    public AppointmentRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
        _dbSet = context.Set<Appointment>();
    }

    public async Task<IEnumerable<Appointment>> GetRangeByStatus(AppointmentStatus status, int startIndex, int count)
    {
        var appointments = await _dbSet
            .Include(a => a.Customer)
            .Where(a => a.Status == status)
            .Skip(startIndex * count)
            .Take(count)
            .OrderByDescending(a => a.Id)
            .ThenBy(a => a.StartTime)
            .ToListAsync();

        return appointments;
    }

    public async Task<int> GetCountByStatus(AppointmentStatus status)
    {
        return await _dbSet
            .CountAsync(a => a.Status == status);
    }

    public async Task<IEnumerable<Appointment>> GetByDateRange(DateTime startTime, DateTime endTime)
    {
        return await _dbSet
            .Include(a => a.Customer)
            .Where(a =>
                ValidStatuses.Contains(a.Status) &&
                a.StartTime >= startTime &&
                a.EndTime <= endTime
            )
            .ToListAsync();
    }

    public async Task<IEnumerable<Appointment>> GetByDateRange(DateTime startTime, DateTime endTime, int customerId)
    {
        return await Find(a =>
            ValidStatuses.Contains(a.Status) &&
            a.StartTime >= startTime &&
            a.EndTime <= endTime
        );
    }

    public async Task<bool> IsOverlapping(DateTime startTime, DateTime endTime)
    {
        return await Contains(a =>
            ValidStatuses.Contains(a.Status) &&
            (
                (startTime >= a.StartTime && startTime < a.EndTime) || // baþlangýç zamaný mevcut randevunun içinde 
                (endTime > a.StartTime && endTime <= a.EndTime) || // bitiþ zamaný mevcut randevunun içinde
                (startTime <= a.StartTime && endTime >= a.EndTime) // yeni randevu mevcut randevuyu kapsýyor
            )
        );
    }

    public new async Task<Appointment> GetById(int id)
    {
        return await _dbSet
            .Include(a => a.Customer)
            .FirstAsync(a => a.Id == id);
    }

}