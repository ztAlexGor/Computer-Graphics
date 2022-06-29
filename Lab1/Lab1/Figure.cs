using System.Drawing;

namespace Lab1
{
    public class Figure
    {
        private List<SceneObject> objects;

        public Figure(List<SceneObject> objects)
        {
            this.objects = objects;
        }
        
        public Figure()
        {
            objects = new List<SceneObject>();
        }

        public void AddObject(SceneObject obj) => objects.Add(obj);

        public List<SceneObject> GetObjects() => objects;


        public void Rotate(float alpha = 0, float beta = 0, float gamma = 0)
        {
            for (int i = 0; i < objects.Count; i++)
            {
                objects[i] = objects[i].Rotate(alpha, beta, gamma);
            }
        }

        public void Scale(float x = 0, float y = 0, float z = 0)
        {
            for (int i = 0; i < objects.Count; i++)
            {
                objects[i] = objects[i].Scale(x, y, z);
            }
        }

        public void Translate(float x = 0, float y = 0, float z = 0)
        {
            for (int i = 0; i < objects.Count; i++)
            {
                objects[i] = objects[i].Translate(x, y, z);
            }
        }
        
        public void Clear() {
            objects.Clear();
        }
    }
}
