namespace EclinicBackend.Dtos
{
    public class CreateChatRoomDto
    {
        public int PatientId { get; set; }
        public string Topic { get; set; } = string.Empty;
    }
}