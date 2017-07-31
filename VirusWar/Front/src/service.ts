export async function createSession(): Promise<GameSession> {
  const response = await fetch('/api/session', {method: 'POST'});
  const sessionID = await response.json();
  return new GameSession(sessionID);
}

export async function getSessions(): Promise<PlayState[]> {
  const response = await fetch(`/api/sessions`, {method: 'GET'});
  return await response.json();
}

export class GameSession {
  constructor(readonly id: string) {}

  async join(): Promise<Player> {
    const response = await fetch(`/api/join/${this.id}`, {method: 'POST'});
    return await response.json();
  }

  async getField(): Promise<Cell[][]>{
    const response = await fetch(`/api/field/${this.id}`, {method: 'GET'});
    return await response.json();
  }

  async getFieldVersion(): Promise<number>{
    const response = await fetch(`/api/field_version/${this.id}`, {method: 'GET'});
    return await response.json();
  }

  async doAction(playerId: string, x: number, y: number): Promise<boolean>{
    const response = await fetch(`/api/action/${this.id}/${playerId}/${x}/${y}`, {method: 'POST'});
    return await response.json()
  }
}

export interface Player {
  id: string;
  order: number;
}

export interface Cell {
  playerId: string;
  alive: boolean;
  avaliableFor: string;
}

export interface PlayState {
  playId: string;
  isPlaying: boolean;
}

export const emptyPlayerId: string = "00000000-0000-0000-0000-000000000000";