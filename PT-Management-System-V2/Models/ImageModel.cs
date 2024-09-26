namespace PT_Management_System_V2.Models
{
    //public class ImageModel
    //{
    //    public int ImageId { get; set; }

    //    public string ImageFilePath { get; set; }

    //    public DateTime DateCreated {  get; set; }

    //    public DateTime? DateDeleted { get; set;} // Nullable datetime
    //}

    public class ImageModel
    {
        public int WeeklyReportImageId { get; set; }

        public int WeeklyReportId { get; set; }

        public int ImageId { get; set; }

        public string ImageFilePath { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime? DateDeleted { get; set;}
    }
}
