import * as React from 'react';
import { Component } from 'react';
import { Redirect } from 'react-router-dom';

import { GameSession, createSession, PlayState, getSessions } from '../service';

export interface LobbyProps {}

interface State {
  readonly creatingSession?: boolean;
  readonly session?: GameSession;
  readonly playerId?: string;
  readonly sessions?: PlayState[];
}

export class Lobby extends Component<LobbyProps, State> {
  constructor(props: LobbyProps, context: any) {
    super(props, context);
    this.state = {};
  }

  sessionIdText: string;

  componentWillMount(){
    setInterval(async () => {
      const sessions = await getSessions();
      this.setState({sessions})
    }, 500);
  }

  render() {
    const {session, playerId, creatingSession} = this.state;
    if (session && playerId) {
      return <Redirect to={`/game/${session.id}/player/${playerId}`} />;
    }

    return (
      <div style={{display: "flex", flexDirection: "column"}}>
        <button type='button'
          disabled={creatingSession}
          onClick={this.onCreateGameClick}>
          Create new game
        </button>
        {
          this.state.sessions == undefined ? "" :
          <div style={{display: "flex", flexDirection: "column"}}>
            {this.state.sessions.map(s => 
              <button key={s.playId} disabled={s.isPlaying || creatingSession} onClick={e => { this.onJoinClick(s.playId) }}>{s.playId}</button>)}
          </div>
        }
        <input type="text" onChange={(e) => this.sessionIdText = e.target.value}/>
        <button onClick={this.onConnectClick} disabled={creatingSession}>Connect to game</button>
      </div>
    );
  }

  private onJoinClick = async (sessionId: string) => {
    this.setState({creatingSession: true});
    const session = new GameSession(sessionId);
    const player = await session.join();
    if (player == null)
      this.setState({creatingSession: false});
    else
      this.setState({creatingSession: false, session, playerId: player.id});
  }

  private onCreateGameClick = async () => {
    this.setState({creatingSession: true});
    const session = await createSession();
    const player = await session.join();
    this.setState({creatingSession: false, session, playerId: player.id});
  }

  private onConnectClick = async () => {
    this.setState({creatingSession: true});
    const session = new GameSession(this.sessionIdText);
    const player = await session.join();
    if (player == null)
      this.setState({creatingSession: false});
    else
      this.setState({creatingSession: false, session, playerId: player.id});
  }
}
