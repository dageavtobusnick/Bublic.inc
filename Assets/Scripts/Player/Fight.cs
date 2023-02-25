using UnityEngine;

public class Fight : MonoBehaviour
{
	private Transform _attackPosition;
	private float _attackRadius;
    private void OnDrawGizmosSelected()
    {
		Gizmos.color = Color.red;
		Gizmos.DrawSphere(_attackPosition.position, _attackRadius);
    }
}
