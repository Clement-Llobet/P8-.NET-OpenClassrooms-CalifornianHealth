﻿using CalifornianHealth.UserInterface.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CalifornianHealth.UserInterface.Pages.Booking;

[Authorize]
public class GetConsultantCalendarModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    public ApiClient _apiClient;
    public List<ConsultantOutputDto> _consultantOutputDto;

    public GetConsultantCalendarModel(ILogger<IndexModel> logger, ApiClient apiClient)
    {
        _logger = logger;
        _apiClient = apiClient;
    }

    public async Task OnGet()
    {
        _consultantOutputDto = await _apiClient.GetAllConsultants();
    }
}
