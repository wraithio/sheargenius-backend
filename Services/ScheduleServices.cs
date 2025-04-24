// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using Microsoft.EntityFrameworkCore;
// using sheargenius_backend.Context;
// using sheargenius_backend.Models;

// namespace sheargenius_backend.Services
// {
//     public class ScheduleServices
//     {
//         private readonly DataContext _dataContext;

//         public ScheduleServices(DataContext dataContext)
//         {
//             _dataContext = dataContext;
//         }
//         public async Task<List<ScheduleModel>> SeeAllSchedules() => await _dataContext.Schedules.ToListAsync();
        

//         public async Task<bool> SetSchedule(ScheduleModel newSchedule){
//             if(FindScheduleByUsername(newSchedule.Username) != null) 
//             {
//                 _dataContext.Schedules.Update(newSchedule);
//                 return await _dataContext.SaveChangesAsync() != 0;
//             }else
//             {
//             await _dataContext.Schedules.AddAsync(newSchedule);
//             return await _dataContext.SaveChangesAsync() != 0;
//             }
//         }

//         public async Task<List<ScheduleModel>> FindScheduleByUsername(string username) => await _dataContext.Schedules.Where(sched => sched.Username == username).ToListAsync();




//     }
// }


// ======================================================================================================

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

    }
}
