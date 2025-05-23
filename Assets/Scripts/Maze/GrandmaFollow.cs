using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 플레이어가 선을 그리면 캐릭터가 따라가는 스크립트
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

        // 스프라이트 렌더러 가져오기
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

    // 선 그리기
    void HandleDrawing()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isDrawing = true;
            pathPoints.Clear();
            lineRenderer.positionCount = 0;
            followIndex = 0;
            hasFinished = false;

            // 플레이어를 시작 위치로 복귀
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
                Debug.Log("벽 위임: 그리지 않음");
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

            // 경로가 없거나 너무 짧으면 리셋
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

    // 플레이어 캐릭터가 선을 따라가게
    void FollowPath()
    {
        // 경로가 없거나 이미 완료된 경우 리턴
        if (pathPoints.Count == 0 || followIndex >= pathPoints.Count || hasFinished)
        {
            return;
        }

        Vector3 target = pathPoints[followIndex];
        Vector2 moveDirection = ((Vector2)target - rb.position).normalized;
        float distanceToTarget = Vector2.Distance(rb.position, target);
        float distance = followSpeed * Time.deltaTime;

        // 목표 지점까지 남은 거리가 이동할 거리보다 작으면 바로 도착점으로 이동
        if (distanceToTarget <= distance)
        {
            Vector2 targetPos2D = new Vector2(target.x, target.y);
            rb.position = targetPos2D;
            followIndex++;

            // Z위치 설정
            Vector3 correctedPos = transform.position;
            correctedPos.z = 0f;
            transform.position = correctedPos;

            return;
        }

        // 충돌 감지 (캐릭터 앞에 Ray를 쏴서 체크)
        RaycastHit2D hit = Physics2D.CircleCast(rb.position, 0.2f, moveDirection, distance, wallLayer);

        // 벽에 부딪히면
        if (hit.collider != null)
        {
            return;
        }

        // 이동 수행 및 위치 저장
        Vector2 nextPos = rb.position + moveDirection * distance;
        rb.MovePosition(nextPos);

        // Z위치 설정
        Vector3 newPos = transform.position;
        newPos.z = 0f;
        transform.position = newPos;

        lastPlayerPos = newPos;
    }

    void LateUpdate()
    {
        // 캐릭터가 화면 밖으로 나가거나 비정상적인 위치로 이동하면 복구
        if (transform.position.magnitude > 1000f || float.IsNaN(transform.position.x) || float.IsNaN(transform.position.y))
        {
            transform.position = lastPlayerPos;
        }

        // 위치가 변경된 후 Z 위치가 항상 0인지
        if (transform.position.z != 0f)
        {
            Vector3 correctedPos = transform.position;
            correctedPos.z = 0f;
            transform.position = correctedPos;
        }
    }

    // 할머니를 만났을 때
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

    // 클리어 시
    private void LoadNextScene()
    {
        SceneManager.LoadScene("VNPart");
    }
}