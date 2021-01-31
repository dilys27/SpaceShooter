
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

*dlb_Player*
- 
- dlb_speed: float = 5f
- dlb_moveHorizontal: float
- dlb_projectile: GameObject
- dlb_projectileSpeed: float = 4f
- dlb_fireRate: float = .50f
- dlb_nextFire: float
- dlb_rb: Rigibody2D
- **dlb_gameManager: dlb_GameManager**
- **dlb_capsule: dlb_Capsule**
- dlb_halo: UnityEditor.SerializedObject
- Start(): void
- Update(): void
- FixedUpdate(): void
- OnTriggerEnter2D(collision: Collider2D): void
- dlb_Move(): void
- dlb_Fire(): void
- dlb_Shoot(): void
- dlb_ApplyBonus(): void

*dlb_BlockScreen*
- 
- dlb_cam: Camera
- dlb_height: float
- dlb_width: float
- Start(): void
- Update(): void

*dlb_Intruder (ennemi)*
- 
- dlb_points: int = 10
- dlb_life: int = 3;
- dlb_maxSpeed: float = 3f
- dlb_minSpeed: float = 1f
- dlb_speed: float
- dlb_arret: bool = true
- dlb_boundsAttackArea: Bounds
- dlb_target: Vector3
- **dlb_projectile: GameObject**
- dlb_projectileSpeed: float = 4f
- dlb_fireRate: float = .50f
- dlb_nextFire: float
- **dlb_capsule: GameObject**
- dlb_capsuleSpeed: float = 2f
- dlb_rb: Rigidbody2D
- **dlb_gameManager: dlb_GameManager**
- cam: Camera
- height: float
- width: float
- Start(): void
- Update(): void
- OnTriggerEnter2D(collision: Collider2D): void
- dlb_Move(): IEnumerator
- dlb_Fire(): void
- dlb_Shoot(): void
- dlb_LastEnemy(): void

*dlb_Bullet*
- 
- life: float = 5f
- Update(): void

*dlb_Capsule*
- 
- dlb_bonus: enum Bonus
- color1: Color
- color2: Color
- color3: Color
- dlb_delai: float
- dlb_cam: Camera
- dlb_height: float
- dlb_width: float
- Start(): void
- Update(): void

*dlb_GameManager*
- 
- dlb_state : enum States
- dlb_wave: int
- dlb_score: int
- dlb_lives: int
- dlb_waveTxt: Text
- dlb_scoreTxt Text
- dlb_livesTxt: Text
- dlb_messageTxt: Text
- **dlb_player: GameObject** 
- **dlb_intruder: GameObject** 
- dlb_boom: GameObject
- cam: Camera
- height, width: float
- dlb_waitToStart: GameObject
- dlb_networkPanel: GameObject
- **dlb_networkManager: dlb_NetworkManager**
- dlb_spawnArea: GameObject
- dlb_widthArea, dlb_xArea, dlb_yArea: float
- dlb_attackArea: GameObject
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
- dlb_gameIsPaused: bool = false
- dlb_pauseTxt: Text
- Start(): void
- Update(): void
- dlb_PauseGame(): void

*dlb_NetworkManager*
- 
Scores: struct [Serializable]
- datas: Score[]
Score: struct [Serializable]
- id: int
- pseudo: string
- score: int
- string: uuid
- URL_REST_API: const string
- leaderboard: Text
- pseudoField: InputField
- pseudoLabel: Text
- playerUUID: string
- playerPseudo: string
- Start(): void
- PseudoFieldOnChange(): void
- SendScore(score: int): void
- SendScoresToNetwork(score: int): IEnumerator
- LoadScores(): void
- LoadScoresFromNetwork(): void

## Assets Sprites
https://www.kenney.nl/assets/space-shooter-extension
