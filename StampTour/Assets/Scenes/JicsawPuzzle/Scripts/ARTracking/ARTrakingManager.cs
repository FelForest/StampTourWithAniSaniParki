using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace JicsawPuzzle
{
    public class ARTrackingManager : MonoBehaviour
    {
        [SerializeField]
        ARTrackedImageManager m_TrackedImageManager;

        void OnEnable() => m_TrackedImageManager.trackedImagesChanged += OnChanged;

        void OnDisable() => m_TrackedImageManager.trackedImagesChanged -= OnChanged;

        void OnChanged(ARTrackedImagesChangedEventArgs eventArgs)
        {
            if (eventArgs.added.Count > 0)
                JicsawPuzzleManager.Instance.OnTrackingAddedEvent(eventArgs.added[^1].referenceImage.name, eventArgs.added[^1].transform);

            foreach (var updatedImage in eventArgs.updated)
            {
                if (updatedImage.trackingState == TrackingState.Tracking || updatedImage.trackingState == TrackingState.None)
                    JicsawPuzzleManager.Instance.OnTrackingUpdateEvent(updatedImage.referenceImage.name, updatedImage.transform);
                else if (updatedImage.trackingState == TrackingState.Limited)
                    JicsawPuzzleManager.Instance.OnTrackingRemovedEvent(updatedImage.referenceImage.name);

            }

            foreach (var removedImage in eventArgs.removed)
            {
                JicsawPuzzleManager.Instance.OnTrackingRemovedEvent(removedImage.referenceImage.name);
            }
        }
    }
}