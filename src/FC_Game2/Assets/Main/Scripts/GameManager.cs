using UnityEngine;

namespace Main
{
    public class GameManager : MonoBehaviour
    {
        static public GameManager A;
        
        private void Awake()
        {
            A = this;
        }

        public void Start(){

        }
    }
}
