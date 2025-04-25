
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using sheargenius_backend.Context;
using sheargenius_backend.Models;

namespace sheargenius_backend.Services
{
    public class ScheduleServices
    {
        private readonly DataContext _dataContext;

        public ScheduleServices(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<List<ScheduleModel>> SeeAllSchedules()
            => await _dataContext.Schedules.ToListAsync();

        public async Task<bool> SetSchedule(ScheduleModel newSchedule)
        {
            var existing = await _dataContext.Schedules.FirstOrDefaultAsync(s => s.Username == newSchedule.Username);
            if (existing != null)
            {
                existing.Days = newSchedule.Days;
                existing.Times = newSchedule.Times;
                _dataContext.Schedules.Update(existing);
            }
            else
            {
                await _dataContext.Schedules.AddAsync(newSchedule);
            }

            return await _dataContext.SaveChangesAsync() != 0;
        }

        public async Task<ScheduleModel?> GetScheduleByUserId(int id)
        {
            return await _dataContext.Schedules.FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<List<ScheduleModel>> GetScheduleByUsername(string username)
        {
            return await _dataContext.Schedules
                .Where(s => s.Username == username)
                .ToListAsync();
        }

        public async Task<bool> DeleteSchedule(int id)
        {
            var schedule = await _dataContext.Schedules.FindAsync(id);
            if (schedule == null) return false;

            _dataContext.Schedules.Remove(schedule);
            return await _dataContext.SaveChangesAsync() > 0;
        }

       
        public async Task<bool> UpdateSchedule( ScheduleModel updatedSchedule)
        {
            var existingSchedule = await _dataContext.Schedules.FindAsync(updatedSchedule.Id);

            if (existingSchedule == null)
                return false;


            existingSchedule.Username = updatedSchedule.Username;
            existingSchedule.Days = updatedSchedule.Days;
            existingSchedule.Times = updatedSchedule.Times;

            try
            {
                _dataContext.Schedules.Update(existingSchedule);
                await _dataContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
public async Task<bool> CheckAvailability(string username, string day, string time)
{
    var userSchedule = await _dataContext.Schedules
        .FirstOrDefaultAsync(s => s.Username == username);

    if (userSchedule == null)
        return false; 

    var normalizedDay = day.Trim().ToLower();
    var normalizedTime = time.Trim().ToLower();

    var isAvailableDay = userSchedule.Days != null &&
        userSchedule.Days.Any(daysString =>
            daysString.Split(',')
                .Select(d => d.Trim().ToLower())
                .Contains(normalizedDay)
        );

    var isAvailableTime = userSchedule.Times != null &&
        userSchedule.Times.Any(timesString =>
            timesString.Split(',')
                .Select(t => t.Trim().ToLower())
                .Contains(normalizedTime)
        );

    return isAvailableDay && isAvailableTime;
}

    }
}
