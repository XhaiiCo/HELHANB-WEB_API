namespace Domain;

public class Conversation
{
    public int Id { get; set; }
    public User User1 { get; set; }
    public User User2 { get; set; }

    private List<Message> _Messages { get; set; }

    public List<Message> Messages
    {
        get => _Messages;
        set { value.ForEach(message => AddMessage(message)); }
    }

    private bool AddMessage(Message message)
    {
        if (User1.Id != message.Sender.Id && User2.Id != message.Sender.Id) return false;
            
        this.Messages.Add(message);
        return true;
    }
}