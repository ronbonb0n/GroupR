public interface IDroneState
{
    IDroneState DoState(DroneController drone);
    void onEnter(DroneController drone);
    void onExit(DroneController drone);
}