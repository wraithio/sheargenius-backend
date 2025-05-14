
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Core;
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

        public async Task<List<ScheduleModel>> SeeAllSchedules() => await _dataContext.Schedules.ToListAsync();
        public async Task<List<RequestModel>> SeeAllRequests() => await _dataContext.Requests.ToListAsync();

        public async Task<bool> SetSchedule(ScheduleModel newSchedule)
        {
            var existing = await _dataContext.Schedules.FirstOrDefaultAsync(s => s.Username == newSchedule.Username);
            if (existing != null)
            {
                existing.MondayTimes = newSchedule.MondayTimes;
                existing.TuesdayTimes = newSchedule.TuesdayTimes;
                existing.WednesdayTimes = newSchedule.WednesdayTimes;
                existing.ThursdayTimes = newSchedule.ThursdayTimes;
                existing.FridayTimes = newSchedule.FridayTimes;
                existing.SaturdayTimes = newSchedule.SaturdayTimes;
                existing.SundayTimes = newSchedule.SundayTimes;
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

        public async Task<bool> UpdateSchedule(ScheduleModel updatedSchedule)
        {
            var existingSchedule = await _dataContext.Schedules.FindAsync(updatedSchedule.Id);

            if (existingSchedule == null)
                return false;

            existingSchedule.Username = updatedSchedule.Username;
            existingSchedule.MondayTimes = updatedSchedule.MondayTimes;
            existingSchedule.TuesdayTimes = updatedSchedule.TuesdayTimes;
            existingSchedule.WednesdayTimes = updatedSchedule.WednesdayTimes;
            existingSchedule.ThursdayTimes = updatedSchedule.ThursdayTimes;
            existingSchedule.FridayTimes = updatedSchedule.FridayTimes;
            existingSchedule.SaturdayTimes = updatedSchedule.SaturdayTimes;
            existingSchedule.SundayTimes = updatedSchedule.SundayTimes;

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

        // public async Task<bool> CheckAvailability(string day, string time, string BarberName)
        // {
        //     var userSchedule = await _dataContext.Schedules
        //         .FirstOrDefaultAsync(s => s.Username == BarberName);

        //     if (userSchedule == null)
        //         return false;


        //     var isAvailableDay = userSchedule.Days != null &&
        //         userSchedule.Days.Any(daysString =>
        //             daysString.Split(',')
        //                 .Select(d => d.Trim().ToLower())
        //                 .Contains(day)
        //         );

        //     var isAvailableTime = userSchedule.Times != null &&
        //         userSchedule.Times.Any(timesString =>
        //             timesString.Split(',')
        //                 .Select(t => t.Trim().ToLower())
        //                 .Contains(time)
        //         );

        //     var existingRequest = await _dataContext.Requests
        //         .FirstOrDefaultAsync(r => r.BarberName == BarberName && r.Day == day && r.Time == time);
        //     if (existingRequest != null)
        //     {
        //         return false; // Request already exists
        //     }
        
        //     return true;
        // }

        public async Task<ScheduleModel> FilterScheduleByRequest(string BarberName)
        {
            var userSchedule = await _dataContext.Schedules
               .FirstOrDefaultAsync(s => s.Username == BarberName);

            var requests = await _dataContext.Requests
                .Where(r => r.BarberName == BarberName && r.isAccepted == true)
                .ToListAsync();

            var filteredSchedule = userSchedule;

            foreach (var request in requests)
            {
                if (request.Day == "Monday")
                {
                    filteredSchedule.MondayTimes = filteredSchedule.MondayTimes
                        .Where(time => time != request.Time && request.isAccepted == true)
                        .ToArray();
                }
                else if (request.Day == "Tuesday")
                {
                    filteredSchedule.TuesdayTimes = filteredSchedule.TuesdayTimes
                        .Where(time => time != request.Time && request.isAccepted == true)
                        .ToArray();
                }
                else if (request.Day == "Wednesday")
                {
                    filteredSchedule.WednesdayTimes = filteredSchedule.WednesdayTimes
                        .Where(time => time != request.Time && request.isAccepted == true)
                        .ToArray();
                }
                else if (request.Day == "Thursday")
                {
                    filteredSchedule.ThursdayTimes = filteredSchedule.ThursdayTimes
                        .Where(time => time != request.Time && request.isAccepted == true)
                        .ToArray();
                }
                else if (request.Day == "Friday")
                {
                    filteredSchedule.FridayTimes = filteredSchedule.FridayTimes
                        .Where(time => time != request.Time && request.isAccepted == true)
                        .ToArray();
                }
                else if (request.Day == "Saturday")
                {
                    filteredSchedule.SaturdayTimes = filteredSchedule.SaturdayTimes
                        .Where(time => time != request.Time && request.isAccepted == true)
                        .ToArray();
                }
                else if (request.Day == "Sunday")
                {
                    filteredSchedule.SundayTimes = filteredSchedule.SundayTimes
                        .Where(time => time != request.Time && request.isAccepted == true)
                        .ToArray();
                }
            }
            return filteredSchedule;
            //     var requestedTimes = requests
            //    .Select(r => r.Time!);


            //     // Filter out any day or time present in the corresponding request sets.
            //     var filteredMondays = userSchedule.MondayTimes
            //         .Where(time => !requestedTimes.Contains(time))
            //         .ToArray();
            //     var filteredTuesday = userSchedule.TuesdayTimes
            //         .Where(time => !requestedTimes.Contains(time))
            //         .ToArray();
            //     var filteredWednesday = userSchedule.WednesdayTimes
            //         .Where(time => !requestedTimes.Contains(time))
            //         .ToArray();
            //     var filteredThursday = userSchedule.ThursdayTimes
            //         .Where(time => !requestedTimes.Contains(time))
            //         .ToArray();
            //     var filteredFridays = userSchedule.FridayTimes
            //         .Where(time => !requestedTimes.Contains(time))
            //         .ToArray();
            //     var filteredSaturdays = userSchedule.SaturdayTimes
            //         .Where(time => !requestedTimes.Contains(time))
            //         .ToArray();
            //     var filteredSundays = userSchedule.SundayTimes
            //         .Where(time => !requestedTimes.Contains(time))
            //         .ToArray();

            // Return a new schedule model with the filtered arrays.
            // return new ScheduleModel
            // {
            //     Id = userSchedule.Id,
            //     Username = userSchedule.Username,
            //     MondayTimes = filteredMondays,
            //     TuesdayTimes = filteredTuesday,
            //     WednesdayTimes = filteredWednesday,
            //     ThursdayTimes = filteredThursday,
            //     FridayTimes = filteredFridays,
            //     SaturdayTimes = filteredSaturdays,
            //     SundayTimes = filteredSundays,
            // };
        }

        public async Task<bool> SendRequest(RequestModel request)
        {
            // var barberSchedule = await _dataContext.Schedules
            //     .FirstOrDefaultAsync(s => s.Username == request.BarberName);

            var existingRequest = await _dataContext.Requests.FirstOrDefaultAsync(r => r.BarberName == request.BarberName && r.Day == request.Day && r.Time == request.Time);
            if (existingRequest != null)
            {
                return false; // Request already exists
            }
            await _dataContext.Requests.AddAsync(request);
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<List<RequestModel>> FindRequestsByBarberName(string username)
        {
            return await _dataContext.Requests
                .Where(r => r.BarberName == username && r.Time != "404")
                .ToListAsync();
        }
        public async Task<List<RequestModel>> FindRequestsByUsername(string username)
        {
            return await _dataContext.Requests
                .Where(r => r.Username == username)
                .ToListAsync();
        }

        public async Task<bool> DeleteRequest(int id)
        {
            var request = await _dataContext.Requests.FindAsync(id);
            if (request == null) return false;

            _dataContext.Requests.Remove(request);
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> AcceptRequest(int id)
        {
            var request = await _dataContext.Requests.FindAsync(id);
            if (request == null) return false;

            request.isAccepted = true;
            _dataContext.Requests.Update(request);
            return await _dataContext.SaveChangesAsync() > 0;
        }
        public async Task<bool> DeclineRequest(int id)
        {
            var request = await _dataContext.Requests.FindAsync(id);
            if (request == null) return false;

            request.Time = "404";
            _dataContext.Requests.Update(request);
            return await _dataContext.SaveChangesAsync() > 0;
        }

    }
}
