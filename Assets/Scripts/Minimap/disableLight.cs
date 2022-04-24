using UnityEngine;

namespace Minimap
{
   public class DisableLight : MonoBehaviour
   {
      void OnPreCull ()
      {
         var allLights = FindObjectsOfType<Light>();
         foreach (var lig in allLights){
            lig.enabled = false;
         }
      }
     
      void OnPostRender ()
      {
         var allLights = FindObjectsOfType<Light>();
         foreach (var lig in allLights){
            lig.enabled = true;
         }
      }
   }
}
