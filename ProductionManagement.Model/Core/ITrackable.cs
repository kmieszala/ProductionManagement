namespace ProductionManagement.Model.Core
{
    public interface ITrackable
    {
        public DateTime CreationDate { get; set; }

        public DateTime ModificationDate { get; set; }

        public int ModificationUserId { get; set; }
    }
}
