
# SpaceShooter

## Examen CSharp Unity n° 1 - M2 DT DC Paris

## Réaliser un jeu de tir en c# avec Unity

 - Date du dernier commit: **2 février 2021, minuit**
 - Toutes les classes et leurs méthodes que vous créez doivent débuter par vos initiales suivi d’un _ (Ne concerne pas les méthodes standard, telles Start(), Update()...)
 - Utilisation d’Unity 2020.2.1
 - Travail individuel
 - Utilisation du jeu réalisé, Asteroids en cours comme base de travail

## Pour jouer

 1. Pour se déplacer : 
 utiliser les **flèches gauche et droite**
 2. Pour tirer : 
 touche **CTRL** ou **clic gauche de la souris**
 3. Mettre en pause / reprendre :
 touche **P**

## UML simplifié

*dlb_Ship*
- 
- initialSpeed: float
- projectile: GameObject
- projectileSpeed: float
- fireRate: float
- nextFire: float
- rb: Rigibody2D
- Start(): void
- Update(): void
- dlb_Move(): void
- dlb_Fire(): void
- dlb_Shoot(): void 

*dlb_Player*
- 
- initialSpeed: float = 10f
- drag: float = 1
- thrust: float
- projectile: GameObject
- projectileSpeed: float = 4f
- fireRate: float = .25f
- nextFire: float
- rb: Rigibody2D
- Start(): void
- Update(): void
- FixedUpdate(): void
- dlb_Move(): void
- dlb_Fire(): void
- dlb_Shoot(): void 

*dlb_Intruder (ennemi)*
- 
- initialSpeed: float = 5.0f
- speed: Vector2
- points: int = 10
- projectile: GameObject
- projectileSpeed: float = 4f
- fireRate: float = .25f
- nextFire: float
- rb: Rigidbody2D
- gameManager:  GameManager
- Start(): void
- Update(): void
- OnTriggerEnter2D(collision: Collider2D): void
- dlb_Move(): void
- dlb_Fire(): void
- dlb_Shoot(): void 

*dlb_Bullet*
- 
- life: float = 3f
- Update(): void

*dlb_Capsule*
- 
- 

*dlb_GameManager*
- 
- state : States
- wave: int
- score: int
- lives: int
- levelTxt: Text
- scoreTxt Text
- livesTxt: Text
- messageTxt: Text
- player: GameObject 
- intruder: GameObject 
- boom: GameObject
- cam: Camera
- height, width: float
- waitToStart: GameObject
- networkPanel: GameObject
- networkManager: NetworkManager
- Start(): void
- Update(): void
- dlb_LaunchGame(): void
- dlb_InitGame(): void
- dlb_LoadWave(): void
- dlb_UpdateTexts(): void
- dlb_AddScore(points: int): void
- dlb_EndOfWave(): void
- dlb_NextWave(): IEnumerator
- dlb_KillPlayer(): void
- dlb_PlayerAgain(): IEnumerator
- dlb_GameOver(): void

*dlb_PauseManager*
- 
- gameIsPaused: bool = false
- pauseTxt: Text
- Start(): void
- Update(): void
- dlb_PauseGame(): void

*dlb_NetworkManager*
- 
- 

## Assets Sprites
https://www.kenney.nl/assets/space-shooter-extension
