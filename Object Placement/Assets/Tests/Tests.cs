using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class Tests
    {
        private ObjectPlacement objectPlacement;
        // A Test behaves as an ordinary method
        [SetUp]
        public void SetUp()
        {
            GameObject objectPlacementObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/ObjectPlacement"));
            objectPlacement = objectPlacementObject.GetComponent<ObjectPlacement>();
        }

        [TearDown]
        public void Teardown()
        {
            Object.Destroy(objectPlacement.gameObject);
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
    public IEnumerator AsteroidTakesTwoHits()
    {

        yield return new WaitForSeconds(0.1f);

    }
    }
}
