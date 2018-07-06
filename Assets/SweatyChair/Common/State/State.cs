public enum State
{
	None, // Default state
	Logo,
	Tutorial,
	Menu,
	Collection,
	CardMachine,
	Shop,
	PreGame, // Use this for stuff that is needed by the Game. Like picking cards.
	Game, // Use this if game starts
	PauseGame,
	EndGame, // Use this if game ends
	MessageBox,
	Weapons,
	WeaponInfo
}