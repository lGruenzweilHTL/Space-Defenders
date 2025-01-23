using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using PathCreation;
using FirstGearGames.SmoothCameraShaker;

public class Enemy_Explode : MonoBehaviour
{
    #region Vars & Refs
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private ShakeData camShakeData;
    [SerializeField] private GameObject explosion;
    [SerializeField] private AudioSource explosionSound;

    private Transform player;
    private Transform playerShield;

    [SerializeField] private float speed;
    [SerializeField] private int damage;
    [SerializeField] private float range;

    [SerializeField] private PathCreator pathCreator;
    float distanceTravelled;

    bool hasSeenPlayer = false;

    [Header("Scanner")]
    [SerializeField] private Transform Scanner_FarLeftAnchor;
    [SerializeField] private Transform Scanner_NearLeftAnchor;
    [SerializeField] private Transform Scanner_FarRightAnchor;
    [SerializeField] private Transform Scanner_NearRightAnchor; 
    [Space]
    [SerializeField] private LineRenderer Scanner_LeftBorderRenderer;
    [SerializeField] private LineRenderer Scanner_RightBorderRenderer;
    [SerializeField] private LineRenderer Scanner_ScanLineRenderer;

    private float Scanner_InterpolateAmmt;
    [SerializeField] private LayerMask lMask;
    #endregion
    #region Functions
    void Start()
    {
        if (GameObject.Find("ExplosionSoundPlayer") != null) explosionSound = GameObject.Find("ExplosionSoundPlayer").GetComponent<AudioSource>();
        if (GameObject.Find("Player") != null) player = GameObject.Find("Player").transform;
        if (player.GetChild(3) != null) playerShield = player.GetChild(3).transform;
        if (GameObject.Find("PatrolPathCreator") != null) pathCreator = GameObject.Find("PatrolPathCreator").GetComponent<PathCreator>();

        if (pathCreator != null)
        {
            //Subscribed to the pathUpdated event so that we're notified if the path changes during the game
            pathCreator.pathUpdated += OnPathChanged;
        }

        //spawn on the path
        OnPathChanged();
    }
    void Update()
    {
        if(hasSeenPlayer)
        {
            //if the player was seen, chase him and deactivate the scanner
            ChasePlayer();
            DeactivateScanner();
        }
        else
        {
            //if the player hasn't been seen, follow the path and scan for the player
            FollowPatrolPath();
            Scan();
        }

        //explode on playerRespawn
        if (player == null) Explode();
    }

    #region Scanning
    private void Scan()
    {
        #region RenderLines
        //Render left Border
        Scanner_LeftBorderRenderer.positionCount = 2;
        Scanner_LeftBorderRenderer.SetPosition(0, Scanner_NearLeftAnchor.position);
        Scanner_LeftBorderRenderer.SetPosition(1, Scanner_FarLeftAnchor.position);

        //Render right Border
        Scanner_RightBorderRenderer.positionCount = 2;
        Scanner_RightBorderRenderer.SetPosition(0, Scanner_NearRightAnchor.position);
        Scanner_RightBorderRenderer.SetPosition(1, Scanner_FarRightAnchor.position);

        //setting the far position of the scanLine by interpolating between the two borders
        Scanner_InterpolateAmmt = (Scanner_InterpolateAmmt + Time.deltaTime) % 1f;
        Vector2 farInterpolatePos = Vector2.Lerp(Scanner_FarLeftAnchor.position, Scanner_FarRightAnchor.position, Scanner_InterpolateAmmt);
        Vector2 nearInterpolatePos = Vector2.Lerp(Scanner_NearLeftAnchor.position, Scanner_NearRightAnchor.position, Scanner_InterpolateAmmt);

        //Render scanLine
        Scanner_ScanLineRenderer.positionCount = 2;
        Scanner_ScanLineRenderer.SetPosition(0, nearInterpolatePos);
        Scanner_ScanLineRenderer.SetPosition(1, farInterpolatePos);
        #endregion
        #region FindPlayer
        //setting the Raycasts range to the current length of the scanLine
        float scanRange = Vector2.Distance(nearInterpolatePos, farInterpolatePos);
        Vector3 scanDir = farInterpolatePos - nearInterpolatePos;

        RaycastHit2D hit = Physics2D.Raycast(nearInterpolatePos, scanDir, scanRange, lMask, Mathf.Infinity, -Mathf.Infinity);
        if (hit.collider != null)
        {
            hasSeenPlayer = true;
        }
        #endregion
    }
    private void DeactivateScanner()
    {
        Scanner_LeftBorderRenderer.positionCount = 0;
        Scanner_RightBorderRenderer.positionCount = 0;
        Scanner_ScanLineRenderer.positionCount = 0;
    }
    #endregion
    #region Patrol
    private void FollowPatrolPath()
    {
        //follow the path
        distanceTravelled += speed * Time.deltaTime;
        transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled);
        transform.rotation = Quaternion.FromToRotation(Vector3.right, pathCreator.path.GetNormalAtDistance(distanceTravelled));
    }
    void OnPathChanged()
    {
        distanceTravelled = pathCreator.path.GetClosestDistanceAlongPath(transform.position);
    }
    #endregion
    #region Chase
    private void ChasePlayer()
    {
        //look at player
        Vector2 lookDir = player.position - transform.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        if (Vector2.Distance(transform.position, player.position) > range)
        {
            //move to player
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }
        else
        {
            Explode();
        }
    }
    private void Explode()
    {
        explosionSound.Play();
        if (playerStats.cameraShakeEnabled) CameraShakerHandler.Shake(camShakeData);
        ParticleSystem explosionParticles = Instantiate(explosion, transform.position, Quaternion.identity).GetComponent<ParticleSystem>();
        explosionParticles.Play();
        if (playerStats.vibrationsEnabled) Handheld.Vibrate();

        if(playerShield.gameObject.activeSelf == false)
        {
            player.GetComponent<Player_Health>().Player_TakeDamage(damage);
        }
        else
        {
            playerShield.GetComponent<Shield_Health>().Shield_TakeDamage(damage);
        }
        Destroy(gameObject);
    }
    #endregion
    #endregion
}