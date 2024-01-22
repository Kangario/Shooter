using UnityEngine;
public interface ICameraController
{
    public void CameraRotate();
}
public class CameraController : MonoBehaviour , ICameraController
{
    public Transform target; // объект, вокруг которого будет вращаться камера
    public float distance = 10.0f; // начальное расстояние до объекта
    public float minDistance = 1.0f; // минимальное расстояние до объекта
    public float maxDistance = 20.0f; // максимальное расстояние до объекта
    public float xSpeed = 250.0f; // скорость вращения камеры вокруг оси X
    public float ySpeed = 120.0f; // скорость вращения камеры вокруг оси Y

    private float x = 0.0f; // текущий угол вращения вокруг оси X
    private float y = 0.0f; // текущий угол вращения вокруг оси Y

    void LateUpdate()
    {
        CameraRotate();
    }
    public void CameraRotate()
    {
        if (target)
        {
            // Получаем входные данные от мыши
            x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
            y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;

            // Ограничиваем угол вращения по вертикали от -90 до 90 градусов
            y = Mathf.Clamp(y, 80, 80);

            // Вычисляем новое положение камеры в пространстве
            Quaternion rotation = Quaternion.Euler(y, x, 0);
            Vector3 position = rotation * new Vector3(0.0f, 1f, -distance) + target.position;

            // Проверяем, что расстояние от камеры до объекта не выходит за пределы допустимых значений
            if (distance > maxDistance)
                distance = maxDistance;
            else if (distance < minDistance)
                distance = minDistance;

            // Перемещаем камеру в новую позицию
            transform.rotation = rotation;
            transform.position = position;

            // Обрабатываем вращение колесика мыши для изменения расстояния до объекта
            distance -= Input.GetAxis("Mouse ScrollWheel") * 5.0f;
        }
    }
}