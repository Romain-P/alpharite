using System;
using System.Collections.Generic;
using Gameplay.View;
using UnityEngine;

namespace AlphaRite.sdk.wrappers {
    public class MapObjectWrapper: Wrapper {
        private static ActiveObject[] emptyObjectArray = new ActiveObject[] { };
        
        public ActiveObject? retrieve(string name) {
            for (var i = 0; i < refs.viewState?.ActiveObjects.Count; i++) {
                var it = refs.viewState.ActiveObjects.Values[i];

                if (refs.data.GetTypeName(it.TypeId) == name)
                    return it;
            }

            return null;
        }
        
        public List<ActiveObject> retrieveAll(string name, Predicate<ActiveObject> predicate = null) {
            var elems = new List<ActiveObject>();
            
            for (var i = 0; i < refs.viewState?.ActiveObjects.Count; i++) {
                var it = refs.viewState.ActiveObjects.Values[i];

                if (refs.data.GetTypeName(it.TypeId) == name && predicate != null && predicate.Invoke(it))
                    elems.Add(it);
            }

            return elems;
        }
        
        public ActiveObject retrieveNearestObject(string name, Predicate<ActiveObject> predicate = null) {
            ActiveObject nearest = default;
            var lowestDist = float.MaxValue;

            var mousePosition = Input.mousePosition.toDualDimension();
            
            foreach (var o in refs.viewState?.ActiveObjects?.Values ?? emptyObjectArray) {
                if (o.TypeId.Id <= 0 || refs.data.GetTypeName(o.TypeId) != name || predicate != null && !predicate.Invoke(o)) 
                    continue;

                var pos = o.ObjectId.screenPosition().toDualDimension() - mousePosition;
                var dist = pos.magnitude();

                if (dist >= lowestDist || dist < 0) continue;

                lowestDist = dist;
                nearest = o;
            }

            return nearest;
        }
    }
}