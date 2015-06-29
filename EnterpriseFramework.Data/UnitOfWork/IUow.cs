//using EnterpriseFramework.Model;

namespace EnterpriseFramework.Data
{
    /// <summary>
    /// Interface for the EnterpriseFramework.Data "Unit of Work"
    /// </summary>
    public interface IUow
    {
        // Save pending changes to the data store.
        void SaveChanges();

        // Repositories
        //IPersonsRepository Persons { get; }
        //IRepository<Room> Rooms { get; }
        //ISessionsRepository Sessions { get; }
        //IRepository<TimeSlot> TimeSlots { get; }
        //IRepository<Track> Tracks { get; }
        //IAttendanceRepository Attendance { get; }
    }
}