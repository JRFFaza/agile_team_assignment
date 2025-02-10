using UnityEngine;
using CW.Common;

namespace Lean.Touch
{
	/// <summary>This component allows you to transform the current GameObject relative to the specified camera using a twist gesture.</summary>
	[HelpURL(LeanTouch.HelpUrlPrefix + "LeanTwistRotate")]
	[AddComponentMenu(LeanTouch.ComponentPathPrefix + "Twist Rotate")]
	public class LeanTwistRotate : MonoBehaviour
	{
        public enum RotationAxis
        {
            X, // Rotate around X-axis
            Y  // Rotate around Y-axis
        }

        public RotationAxis rotationAxis = RotationAxis.Y; // Default Axis set to Y

        private float axisSwitchThreshold = 10.0f; // Threshold to switch axes
        private float lastTwist = 0.0f;
        
		/// <summary>The method used to find fingers to use with this component. See LeanFingerFilter documentation for more information.</summary>
        public LeanFingerFilter Use = new LeanFingerFilter(true);

		/// <summary>The camera we will be used to calculate relative rotations.
		/// None/null = MainCamera.</summary>
		public Camera Camera { set { _camera = value; } get { return _camera; } } [SerializeField] private Camera _camera;

		/// <summary>Should the rotation be performed relative to the finger center?</summary>
		public bool Relative { set { relative = value; } get { return relative; } } [SerializeField] private bool relative;

		/// <summary>If you want this component to change smoothly over time, then this allows you to control how quick the changes reach their target value.
		/// -1 = Instantly change.
		/// 1 = Slowly change.
		/// 10 = Quickly change.</summary>
		public float Damping { set { damping = value; } get { return damping; } } [SerializeField] private float damping = -1.0f;

		[SerializeField]
		private Vector3 remainingTranslation;

		[SerializeField]
		private Quaternion remainingRotation = Quaternion.identity;

		/// <summary>If you've set Use to ManuallyAddedFingers, then you can call this method to manually add a finger.</summary>
		public void AddFinger(LeanFinger finger)
		{
			Use.AddFinger(finger);
		}

		/// <summary>If you've set Use to ManuallyAddedFingers, then you can call this method to manually remove a finger.</summary>
		public void RemoveFinger(LeanFinger finger)
		{
			Use.RemoveFinger(finger);
		}

		/// <summary>If you've set Use to ManuallyAddedFingers, then you can call this method to manually remove all fingers.</summary>
		public void RemoveAllFingers()
		{
			Use.RemoveAllFingers();
		}

#if UNITY_EDITOR
		protected virtual void Reset()
		{
			Use.UpdateRequiredSelectable(gameObject);
		}
#endif

		protected virtual void Awake()
		{
			Use.UpdateRequiredSelectable(gameObject);
		}

		protected virtual void Update()
		{
            // Get the fingers we want to use
            var fingers = Use.UpdateAndGetFingers();

            // Calculate the rotation values based on these fingers
            var twistDegrees = LeanGesture.GetTwistDegrees(fingers);
       
            if (twistDegrees != 0.0f)
            {
                // Check the direction of the twist (change of direction)
                if (Mathf.Abs(twistDegrees - lastTwist) > axisSwitchThreshold)
                {
                    // Switch axis if the twist direction changes significantly
                    if (rotationAxis == RotationAxis.X)
                    {
                        rotationAxis = RotationAxis.Y; // Switch to Y-axis
                    }
                    else
                    {
                        rotationAxis = RotationAxis.X; // Switch to X-axis
                    }
                }

                // Store the current twist for future comparison
                lastTwist = twistDegrees;

                // Rotate based on the selected axis
                if (transform is RectTransform)
                {
                    RotateUI(twistDegrees);
                }
                else
                {
                    Rotate(twistDegrees);
                }
            }

            // Store
            var oldPosition = transform.localPosition;
            var oldRotation = transform.localRotation;

            // Increment
            remainingTranslation += transform.localPosition - oldPosition;
			remainingRotation    *= Quaternion.Inverse(oldRotation) * transform.localRotation;

			// Get t value
			var factor = CwHelper.DampenFactor(damping, Time.deltaTime);

			// Dampen remainingDelta
			var newRemainingTranslation = Vector3.Lerp(remainingTranslation, Vector3.zero, factor);
			var newRemainingRotation    = Quaternion.Slerp(remainingRotation, Quaternion.identity, factor);

			// Shift this transform by the change in delta
			transform.localPosition = oldPosition + remainingTranslation - newRemainingTranslation;
			transform.localRotation = oldRotation * Quaternion.Inverse(newRemainingRotation) * remainingRotation;

			// Update remainingDelta with the dampened value
			remainingTranslation = newRemainingTranslation;
			remainingRotation    = newRemainingRotation;
		}

		protected virtual void TranslateUI(float twistDegrees, Vector2 twistScreenCenter)
		{
			var camera = _camera;

			if (camera == null)
			{
				var canvas = transform.GetComponentInParent<Canvas>();

				if (canvas != null && canvas.renderMode != RenderMode.ScreenSpaceOverlay)
				{
					camera = canvas.worldCamera;
				}
			}

			// Screen position of the transform
			var screenPoint = RectTransformUtility.WorldToScreenPoint(camera, transform.position);

			// Twist screen point around the twistScreenCenter by twistDegrees
			var twistRotation = Quaternion.Euler(0.0f, 0.0f, twistDegrees);
			var screenDelta   = twistRotation * (screenPoint - twistScreenCenter);

			screenPoint.x = twistScreenCenter.x + screenDelta.x;
			screenPoint.y = twistScreenCenter.y + screenDelta.y;

			// Convert back to world space
			var worldPoint = default(Vector3);

			if (RectTransformUtility.ScreenPointToWorldPointInRectangle(transform.parent as RectTransform, screenPoint, camera, out worldPoint) == true)
			{
				transform.position = worldPoint;
			}
		}

		protected virtual void Translate(float twistDegrees, Vector2 twistScreenCenter)
		{
			// Make sure the camera exists
			var camera = CwHelper.GetCamera(_camera, gameObject);

			if (camera != null)
			{
				// Screen position of the transform
				var screenPoint = camera.WorldToScreenPoint(transform.position);

				// Twist screen point around the twistScreenCenter by twistDegrees
				var twistRotation = Quaternion.Euler(0.0f, 0.0f, twistDegrees);
				var screenDelta   = twistRotation * ((Vector2)screenPoint - twistScreenCenter);

				screenPoint.x = twistScreenCenter.x + screenDelta.x;
				screenPoint.y = twistScreenCenter.y + screenDelta.y;

				// Convert back to world space
				transform.position = camera.ScreenToWorldPoint(screenPoint);
			}
			else
			{
				Debug.LogError("Failed to find camera. Either tag your cameras MainCamera, or set one in this component.", this);
			}
		}

		protected virtual void RotateUI(float twistDegrees)
		{
            //transform.rotation *= Quaternion.Euler(0.0f, 0.0f, twistDegrees);

            if (rotationAxis == rotationAxis.X)
            {
                transform.rotation *= Quaternion.Euler(twistDegrees, 0.0f, 0.0f); // Rotate only along X-axis
            }
            else if (rotationAxis == rotationAxis.Y)
            {
                transform.rotation *= Quaternion.Euler(0.0f, twistDegrees, 0.0f); // Rotate only along Y-axis
            }
        }

		protected virtual void Rotate(float twistDegrees)
		{
			// Make sure the camera exists
			var camera = CwHelper.GetCamera(_camera, gameObject);

			if (camera != null)
			{
                Vector3 rotationAxisVector = Vector3.zero;

                // Smoothly switch between X and Y axis
                if (RotatinAxis == RotationAxis.X)
                {
                    rotationAxisVector = transform.right; // X axis (local space)
                }
                else if (RotationAxis == RotationAxis.Y)
                {
                    RotatinAxisVector = transform.up; // Y axis (local space)
                }

                //var axis = transform.InverseTransformDirection(camera.transform.forward);
                
				transform.rotation *= Quaternion.AngleAxis(twistDegrees, rotationAxisVector);
			}
			else
			{
				Debug.LogError("Failed to find camera. Either tag your cameras MainCamera, or set one in this component.", this);
			}
		}
	}
}

#if UNITY_EDITOR
namespace Lean.Touch.Editor
{
	using UnityEditor;
	using TARGET = LeanTwistRotate;

	[CanEditMultipleObjects]
	[CustomEditor(typeof(TARGET))]
	public class LeanTwistRotate_Editor : CwEditor
	{
		protected override void OnInspector()
		{
			TARGET tgt; TARGET[] tgts; GetTargets(out tgt, out tgts);

			Draw("Use");
			Draw("_camera", "The camera we will be used to calculate relative rotations.\n\nNone/null = MainCamera.");
			Draw("relative", "Should the rotation be performed relative to the finger center?");
			Draw("damping", "If you want this component to change smoothly over time, then this allows you to control how quick the changes reach their target value.\n\n-1 = Instantly change.\n\n1 = Slowly change.\n\n10 = Quickly change.");
		}
	}
}
#endif