using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GrandmaFollow : MonoBehaviour
{
    public GameObject completePanel;

    public float speed = 2f;
    public LayerMask obstacleLayer;

    private List<Vector3> path = new List<Vector3>();
    private int targetIndex = 0;
    private bool isFollowing = false;

    private Vector3 startPosition;

    private bool hasFinished = false;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        if (!isFollowing || path.Count == 0 || targetIndex >= path.Count)
        {
            return;
        }

        Vector3 target = path[targetIndex];
        Vector3 direction = (target - transform.position).normalized;
        float distance = speed * Time.deltaTime;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, distance, obstacleLayer);
        if (hit.collider != null)
        {
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, target, distance);

        if (Vector3.Distance(transform.position, target) < 0.05f)
        {
            targetIndex++;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasFinished)
        {
            return;
        }

        if (other.CompareTag("Exit"))
        {
            hasFinished = true;
            Debug.Log("µµÂø");
            completePanel.SetActive(true);

            SceneManager.LoadScene("VNPart");
        }
    }

    public void AddPathPoint(Vector3 point)
    {
        path.Add(point);

        if (!isFollowing)
        {
            isFollowing = true;
            targetIndex = 0;
        }
    }

    public void ResetPath()
    {
        path.Clear();
        isFollowing = false;
        targetIndex = 0;
    }

    public void ReturnToStart()
    {
        ResetPath();
        transform.position = startPosition;
    }

    public void StopFollowing()
    {
        StopAllCoroutines();
    }
}
