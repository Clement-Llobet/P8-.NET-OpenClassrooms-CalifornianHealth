﻿using CalifornianHealth.Infrastructure.Database.Contexts;
using CalifornianHealth.Infrastructure.Database.Entities;

namespace CalifornianHealth.Infrastructure.Database.Repositories.ConsultantCalendarRepository;

public class ConsultantCalendarRepository : IConsultantCalendarRepository
{
    private readonly CalifornianHealthContext _dbContext;
    
    public ConsultantCalendarRepository(CalifornianHealthContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IEnumerable<ConsultantCalendar> FetchConsultantCalendar()
    {
        return _dbContext.ConsultantCalendars.ToList();
    }

    public IEnumerable<ConsultantCalendar> FetchConsultantCalendarsByConsultantId(int id)
    {
        return _dbContext.ConsultantCalendars.Where(cc => cc.ConsultantId == id).ToList();
    }

    public ConsultantCalendar FetchOneConsultantCalendarById(int id)
    {
        return _dbContext.ConsultantCalendars.Where(cc => cc.Id == id).FirstOrDefault()!;
    }

    public int UpdateConsultantCalendar(ConsultantCalendar consultantCalendar)
    {
        var entityUpdated = _dbContext.ConsultantCalendars.Update(consultantCalendar);
        _dbContext.SaveChanges();

        return entityUpdated.Entity.Id;
    }
}
