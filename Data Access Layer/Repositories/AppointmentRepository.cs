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

    public AppointmentRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
        _dbSet = context.Set<Appointment>();
    }

    public async Task<IEnumerable<Appointment>> GetRangeByStatus(AppointmentStatus status, int startIndex, int count)
    {

        var appointments = await _dbSet
            .Include(a => a.customer)
            .Where(a => a.status == status)
            .Skip(startIndex * count)
            .Take(count)
            .OrderByDescending(a => a.id)
            .ThenBy(a => a.startTime)
            .ToListAsync();

        return appointments;
    }

    public async Task<int> GetCountByStatus(AppointmentStatus status)
    {
        return await _dbSet
            .CountAsync(a => a.status == status);
    }

    public async Task<IEnumerable<Appointment>> GetByDateRange(DateTime startTime, DateTime endTime)
    {
        return await _dbSet
            .Where(a => a.startTime >= startTime && a.endTime <= endTime)
            .ToListAsync();
    }

    public async Task<bool> IsOverlapping(DateTime startTime, DateTime endTime)
    {
        return await Contains(a =>
                (startTime >= a.startTime &&
                 startTime < a.endTime) || // Yeni baþlangýç zamaný mevcut randevunun içinde mi?
                (endTime > a.startTime && endTime <= a.endTime) || // Yeni bitiþ zamaný mevcut randevunun içinde mi?
                (startTime <= a.startTime && endTime >= a.endTime) // Yeni randevu mevcut randevuyu kapsýyor mu?
        );
    }
}