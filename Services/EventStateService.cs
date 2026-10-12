using EventEase.Models;

namespace EventEase.Services;

public class EventStateService
{
    public UserSession CurrentSession { get; private set; } = new();

    // Key: EventId, Value: List of confirmed registrations
    private readonly Dictionary<int, List<RegistrationModel>> _attendanceRegistry = new();

    // Event capacity caps
    private readonly Dictionary<int, int> _capacityMap = new()
    {
        { 101, 150 },
        { 102, 300 },
        { 103, 80  }
    };

    public event Action? OnChange;

    public void SetUserSession(string name, string email)
    {
        CurrentSession.CurrentUserName = name;
        CurrentSession.CurrentUserEmail = email;
        NotifyStateChanged();
    }

    public void ClearSession()
    {
        CurrentSession = new UserSession();
        NotifyStateChanged();
    }

    public bool TryRegister(RegistrationModel model, out string failureReason)
    {
        if (!_attendanceRegistry.ContainsKey(model.EventId))
        {
            _attendanceRegistry[model.EventId] = new List<RegistrationModel>();
        }

        int currentCount = GetConfirmedAttendeeCount(model.EventId);
        int maxCapacity = GetMaxCapacity(model.EventId);

        if (currentCount + model.TicketCount > maxCapacity)
        {
            failureReason = $"Only {maxCapacity - currentCount} spots remain for this event.";
            return false;
        }

        _attendanceRegistry[model.EventId].Add(model);
        SetUserSession(model.FullName, model.Email);
        failureReason = string.Empty;
        NotifyStateChanged();
        return true;
    }

    public int GetConfirmedAttendeeCount(int eventId) =>
        _attendanceRegistry.TryGetValue(eventId, out var list) ? list.Sum(r => r.TicketCount) : 0;

    public int GetMaxCapacity(int eventId) =>
        _capacityMap.TryGetValue(eventId, out int cap) ? cap : 100;

    public IReadOnlyList<RegistrationModel> GetRegistrations(int eventId) =>
        _attendanceRegistry.TryGetValue(eventId, out var list) ? list.AsReadOnly() : Array.Empty<RegistrationModel>();

    private void NotifyStateChanged() => OnChange?.Invoke();
}