export async function createSession(): Promise<GameSession> {
  const response = await fetch('/api/session', {method: 'POST'});
  const sessionID = await response.json();
  return new GameSession(sessionID);
}

export class GameSession {
  constructor(readonly id: string) {}

  async join(): Promise<Player> {
    const response = await fetch(`/api/join/${this.id}`, {method: 'POST'});
    return await response.json();
  }
}

interface Player {
  id: string;
  order: number;
}
