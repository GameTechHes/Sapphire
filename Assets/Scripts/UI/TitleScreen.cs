using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class TitleScreen : MonoBehaviour
    {
        public void Create()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        public void Join()
        {

        }

        public void Quit()
        {
            Application.Quit();
        }
    }
}
