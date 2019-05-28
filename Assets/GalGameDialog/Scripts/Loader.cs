using UnityEngine;

namespace GalGameDialog
{
    public class Loader : MonoBehaviour
    {
        public GameObject gameConfgs;          //GameManager prefab to instantiate.
        public GameObject gameManager;			//GameManager prefab to instantiate.
		public GameObject soundManager;			//SoundManager prefab to instantiate.
		
		
		void Awake ()
        {
            //Check if a GameManager has already been assigned to static variable GameManager.instance or if it's still null
            if (GameConfig.instance == null)

                //Instantiate gameManager prefab
                Instantiate(gameConfgs);

            //Check if a GameManager has already been assigned to static variable GameManager.instance or if it's still null
            if (GameManager.instance == null)
				
				//Instantiate gameManager prefab
				Instantiate(gameManager);
			
			//Check if a SoundManager has already been assigned to static variable GameManager.instance or if it's still null
			//if (SoundManager.instance == null)
				
				//Instantiate SoundManager prefab
				//Instantiate(soundManager);
		}
	}
}