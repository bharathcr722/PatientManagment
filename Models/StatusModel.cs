namespace PatientManagment.Models
{
    public class StatusModel
    {
        public int StatusCode { get; set; }
        public Status Status { get; set; }
        public string Message { get; set; }
        public StatusModel(Status status)
        {
            Status = status;
        }
        public StatusModel(){}
    }
}
