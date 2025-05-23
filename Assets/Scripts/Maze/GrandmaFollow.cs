using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// �÷��̾ ���� �׸��� ĳ���Ͱ� ���󰡴� ��ũ��Ʈ
/// </summary>

public class GrandmaFollow : MonoBehaviour
{
    [Header("Reference")]
    public LineRenderer lineRenderer;
    public Transform player;
    public Transform startPos;
    public LayerMask wallLayer;
    public StartManager startManager;
    public GameObject completePanel;
    public SpriteRenderer spriteRenderer;

    [Header("Settings")]
    public float followSpeed = 15f;
    public float pointSpacing = 0.1f;
    public float wallCheckRadius = 0.05f;
    public float arrivalThreshold = 0.05f;

    private List<Vector3> pathPoints = new List<Vector3>();
    private int followIndex = 0;
    private bool isDrawing = false;
    private bool hasFinished = false;
    private Rigidbody2D rb;
    private Vector3 lastPlayerPos;
    private Camera mainCamera;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (rb == null)
        { 
            rb = gameObject.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0f;
        }

        // ��������Ʈ ������ ��������
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;

        lastPlayerPos = transform.position;
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (startManager.isStart)
        {
            HandleDrawing();
            FollowPath();
        }
    }

    // �� �׸���
    void HandleDrawing()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isDrawing = true;
            pathPoints.Clear();
            lineRenderer.positionCount = 0;
            followIndex = 0;
            hasFinished = false;

            // �÷��̾ ���� ��ġ�� ����
            if (startManager != null && startPos != null)
            {
                Vector3 resetPos = startPos.position;
                resetPos.z = 0f;
                transform.position = resetPos;
                lastPlayerPos = resetPos;
            }
        }

        if (Input.GetMouseButton(0) && isDrawing)
        {
            Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0f;

            if (Physics2D.OverlapCircle(mousePos, wallCheckRadius, wallLayer) != null)
            {
                Debug.Log("�� ����: �׸��� ����");
            }
            else
            {
                if (pathPoints.Count == 0 || Vector3.Distance(pathPoints[pathPoints.Count - 1], mousePos) > pointSpacing)
                {
                    pathPoints.Add(mousePos);

                    Vector3 fixedPoint = new Vector3(mousePos.x, mousePos.y, 0f);

                    lineRenderer.positionCount = pathPoints.Count;
                    lineRenderer.SetPosition(pathPoints.Count - 1, fixedPoint);
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDrawing = false;

            // ��ΰ� ���ų� �ʹ� ª���� ����
            if (pathPoints.Count < 2)
            {
                pathPoints.Clear();
                lineRenderer.positionCount = 0;
                followIndex = 0;

                if (startManager != null && startPos != null)
                {
                    Vector3 resetPos = startPos.position;
                    resetPos.z = 0f;
                    transform.position = resetPos;
                    lastPlayerPos = resetPos;
                }
            }
        }
    }

    // �÷��̾� ĳ���Ͱ� ���� ���󰡰�
    void FollowPath()
    {
        // ��ΰ� ���ų� �̹� �Ϸ�� ��� ����
        if (pathPoints.Count == 0 || followIndex >= pathPoints.Count || hasFinished)
        {
            return;
        }

        Vector3 target = pathPoints[followIndex];
        Vector2 moveDirection = ((Vector2)target - rb.position).normalized;
        float distanceToTarget = Vector2.Distance(rb.position, target);
        float distance = followSpeed * Time.deltaTime;

        // ��ǥ �������� ���� �Ÿ��� �̵��� �Ÿ����� ������ �ٷ� ���������� �̵�
        if (distanceToTarget <= distance)
        {
            Vector2 targetPos2D = new Vector2(target.x, target.y);
            rb.position = targetPos2D;
            followIndex++;

            // Z��ġ ����
            Vector3 correctedPos = transform.position;
            correctedPos.z = 0f;
            transform.position = correctedPos;

            return;
        }

        // �浹 ���� (ĳ���� �տ� Ray�� ���� üũ)
        RaycastHit2D hit = Physics2D.CircleCast(rb.position, 0.2f, moveDirection, distance, wallLayer);

        // ���� �ε�����
        if (hit.collider != null)
        {
            return;
        }

        // �̵� ���� �� ��ġ ����
        Vector2 nextPos = rb.position + moveDirection * distance;
        rb.MovePosition(nextPos);

        // Z��ġ ����
        Vector3 newPos = transform.position;
        newPos.z = 0f;
        transform.position = newPos;

        lastPlayerPos = newPos;
    }

    void LateUpdate()
    {
        // ĳ���Ͱ� ȭ�� ������ �����ų� ���������� ��ġ�� �̵��ϸ� ����
        if (transform.position.magnitude > 1000f || float.IsNaN(transform.position.x) || float.IsNaN(transform.position.y))
        {
            transform.position = lastPlayerPos;
        }

        // ��ġ�� ����� �� Z ��ġ�� �׻� 0����
        if (transform.position.z != 0f)
        {
            Vector3 correctedPos = transform.position;
            correctedPos.z = 0f;
            transform.position = correctedPos;
        }
    }

    // �ҸӴϸ� ������ ��
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasFinished)
        {
            return;
        }

        if (other.CompareTag("Exit"))
        {
            hasFinished = true;

            if (completePanel != null)
            {
                completePanel.SetActive(true);
            }

            Invoke("LoadNextScene", 1.0f);
        }
    }

    // Ŭ���� ��
    private void LoadNextScene()
    {
        SceneManager.LoadScene("VNPart");
    }
}