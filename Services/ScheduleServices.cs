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
        public async Task<List<ScheduleModel>> SeeAllSchedules() => await _dataContext.Schedules.ToListAsync();
        

        public async Task<bool> SetSchedule(ScheduleModel newSchedule){
            if(FindScheduleByUsername(newSchedule.Username) != null) 
            {
                _dataContext.Schedules.Update(newSchedule);
                return await _dataContext.SaveChangesAsync() != 0;
            }else
            {
            await _dataContext.Schedules.AddAsync(newSchedule);
            return await _dataContext.SaveChangesAsync() != 0;
            }
        }

        public async Task<List<ScheduleModel>> FindScheduleByUsername(string username) => await _dataContext.Schedules.Where(sched => sched.Username == username).ToListAsync();

    }
}