using Data_Access_Layer.Data;
using Microsoft.EntityFrameworkCore;
using Models.Enums;
using Models.Models;

namespace Data_Access_Layer.Repositories;

public class AppointmentRepository : Repository<Appointment>,IAppointmentRepository
{
    private readonly DbContext _context;
    private readonly DbSet<Appointment> _dbSet;
    public AppointmentRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
        _dbSet = context.Set<Appointment>();
    }
    public IEnumerable<Appointment> GetRangeByStatus(AppointmentStatus status, int startIndex, int count)
    {
        // Step 1: Retrieve the distinct dates for the specified range with the given status
        var dates = _dbSet
            .Where(a => a.status == status)
            .Select(a => a.startTime.Date)
            .Distinct()
            .OrderByDescending(date => date) // Descending order by date to get latest first
            .Skip(startIndex)
            .Take(count)
            .ToList();

        // Step 2: Retrieve the appointments for these dates with the given status, sorted by ID descending
        var appointments = _dbSet
            .Where(a => a.status == status && dates.Contains(a.startTime.Date))
            .OrderByDescending(a => a.id) // Order by ID descending
            .ThenBy(a => a.startTime)     // Then by startTime if needed (optional)
            .ToList();

        return appointments;
    }

    public IEnumerable<Appointment> GetRange(int startIndex, int count)
    {
        // Step 1: Retrieve the distinct dates for the specified range
        var dates = _dbSet
            .Select(a => a.startTime.Date)
            .Distinct()
            .OrderBy(date => date)
            .Skip(startIndex)
            .Take(count)
            .ToList();

        // Step 2: Retrieve the appointments for these dates
        var appointments = _dbSet
            .Where(a => dates.Contains(a.startTime.Date))
            .OrderBy(a => a.startTime)
            .ToList();

        return appointments;
    }

    public int GetAppointmentNumber()
    {
        return _dbSet
            .Select(a => a.startTime.Date)
            .Distinct()
            .Count();
    }

    public int GetCountByStatus(AppointmentStatus status)
    {
        return _dbSet
            .Count(a => a.status == status);
    }

    public IEnumerable<Appointment> GetByDateRange(DateTime startTime, DateTime endTime)
    {
        return _dbSet
            .Where(a => a.startTime >= startTime && a.endTime <= endTime)
            .ToList();
    }

}