using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public Vector3 target;
    public float speed = 1.0f;
    public float arcHeightMod = 0.5f;
    public void SetTaget(Vector3 targetPos)
    {
        target = targetPos;
        speed += (Vector3.Distance(transform.position, target)/5);
    }
    public void StartFlying()
    {
        StartCoroutine(ImFlying());
    }
    private IEnumerator ImFlying()
    {

        Vector3 startPosition = transform.position;

        float distance = Vector3.Distance(startPosition, target);
        arcHeightMod *= distance;

        float _stepScale = speed / distance;
        float progress = 0f;

        bool flying = true;

        while (flying)
        {
            progress = Mathf.Min(progress + Time.deltaTime * _stepScale, 1.0f);

            float parabola = 1.0f - 4.0f * (progress - 0.5f) * (progress - 0.5f);

            Vector3 nextPos = Vector3.Lerp(startPosition, target, progress);
            nextPos.y += parabola * arcHeightMod;

            transform.LookAt(nextPos, transform.forward);
            transform.position = nextPos;

            if (progress == 1.0f)
            {
                flying = false;
            }


            yield return null;
        }

        Destroy(gameObject);
    }


}
