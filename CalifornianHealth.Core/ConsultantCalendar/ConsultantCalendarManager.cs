﻿using CalifornianHealth.Core.ConsultantCalendar.Contracts;
using CalifornianHealth.Infrastructure.Database.Entities;
using CalifornianHealth.Infrastructure.Database.Repositories.AppointmentRepository;
using CalifornianHealth.Infrastructure.Database.Repositories.ConsultantCalendarRepository;

namespace CalifornianHealth.Core.ConsultantCalendar
{
    public class ConsultantCalendarManager : IConsultantCalendarManager
    {
        private readonly IConsultantCalendarRepository _consultantCalendarRepository;
        private readonly IAppointmentRepository _appointmentRepository;

        public ConsultantCalendarManager(IConsultantCalendarRepository consultantCalendarRepository)
        {
            _consultantCalendarRepository = consultantCalendarRepository;
        }

        public List<ConsultantCalendarOutputDto> GetAllConsultantCalendars()
        {
            var request = _consultantCalendarRepository.FetchConsultantCalendar();
            return CreateOutputList(request);
        }

        public List<ConsultantCalendarOutputDto> GetConsultantCalendarsById(int id)
        {
            var request = _consultantCalendarRepository.FetchConsultantCalendarById(id);
            return CreateOutputList(request);
        }

        public int BookAppointment(AppointmentInputDto appointmentInput)
        {
            var request = _consultantCalendarRepository.FetchConsultantCalendarById(appointmentInput.ConsultantId);
            var consultantCalendar = request.FirstOrDefault(x => x.Date == appointmentInput.StartDateTime);

            if (consultantCalendar == null)
                throw new Exception("Consultant not available on this date");

            if (!consultantCalendar.Available)
                throw new Exception("Consultant not available on this date");

            consultantCalendar.Available = false;
            _consultantCalendarRepository.UpdateConsultantCalendar(consultantCalendar);

            var newAppointment = new Appointment
            {
                ConsultantId = appointmentInput.ConsultantId,
                StartDateTime = appointmentInput.StartDateTime,
                EndDateTime = appointmentInput.EndDateTime,
                PatientId = appointmentInput.PatientId
            };

            return _appointmentRepository.CreateAppointment(newAppointment);
        }

        private List<ConsultantCalendarOutputDto> CreateOutputList(IEnumerable<Infrastructure.Database.Entities.ConsultantCalendar> request)
        {
            return request.Select(x => new ConsultantCalendarOutputDto
            {
                Id = x.Id,
                ConsultantId = x.ConsultantId,
                Date = x.Date,
                Available = x.Available
            }).ToList();
        }
    }
}