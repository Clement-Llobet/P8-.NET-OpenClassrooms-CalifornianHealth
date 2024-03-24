﻿using CalifornianHealth.Core.ConsultantCalendar.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace CalifornianHealth.WebAPIs.Calendar.Endpoints;

public static class ConsultantCalendarEndpoints
{
    public static void MapConsultantCalendarEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("api/ConsultantCalendar", GetAllConsultants);
        app.MapGet("api/ConsultantCalendar/{id}", GetConsultantCalendarById);
        app.MapPost("api/ConsultantCalendar", PostAppointment);
    }

    public static async Task<IResult> GetAllConsultants(IConsultantCalendarManager manager)
    {
        var consultantCalendarList = manager.GetAllConsultantCalendars();

        if (consultantCalendarList == null)
            return Results.NotFound("No consultant calendars found");

        return Results.Ok(consultantCalendarList);
    }
    
    public static async Task<IResult> GetConsultantCalendarById(IConsultantCalendarManager manager, int id)
    {
        var consultantCalendar = manager.GetConsultantCalendarsById(id);

        if (consultantCalendar == null)
            return Results.NotFound("No consultant calendars found");

        return Results.Ok(consultantCalendar);
    }

    public static async Task<IResult> PostAppointment(IConsultantCalendarManager manager, [FromBody]AppointmentInputDto appointmentInput)
    {
        Semaphore pool = new(initialCount: 0, maximumCount: 1);

        var createdAppointmentId = manager.BookAppointment(appointmentInput);

        if (createdAppointmentId == 0)
            return Results.BadRequest("Appointment not created");

        pool.Release(releaseCount: 1);

        return Results.Ok(createdAppointmentId);
    }
}