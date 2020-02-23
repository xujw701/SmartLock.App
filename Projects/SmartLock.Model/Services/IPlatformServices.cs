namespace SmartLock.Model.Services
{
    public interface IPlatformServices
    {
        void Call(string phoneNumber);

        void Sms(string phoneNumber);

        void Email(string emailAddress);

        void Exit();

        void Bt();
    }
}
