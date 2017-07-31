import * as React from 'react';
import { Component } from 'react';
import { match } from 'react-router-dom';
import { GameSession, Cell, emptyPlayerId } from '../service'

export interface GameProps {
  readonly match: match<{
    readonly sessionId: string;
    readonly playerId: string;
  }>;
}

export interface GameState {
  field?: Cell[][];
}

export class Game extends Component<GameProps, GameState> {

  fieldVersion: number = -5;

  constructor(props?: GameProps, context?: any){
    super(props, context);
    this.state = {};
  }

  componentDidMount(){
    setInterval(this.fieldUpdate.bind(this), 500);
  }

  async fieldUpdate(){
    var session = new GameSession(this.props.match.params.sessionId);
    var newVersion = await session.getFieldVersion();
    if (newVersion == this.fieldVersion)
      return;
    var newField = await session.getField();
    this.fieldVersion = newVersion;
    this.setState({field: newField});
  }

  render() {
    const {sessionId, playerId} = this.props.match.params;

    if (this.state.field == undefined)
      return <div>Waiting for player..</div>;

    return (
      <div>
        Game {sessionId} as player {playerId} <br/>
        <div style={{display: "flex", flexDirection: "row"}}>
          {this.state.field.map((col, colIndex) => 
            <div key={colIndex} style={{display: "flex", flexDirection: "column"}}>
              {col.map((cell, cellIndex) => <CellComponent key={colIndex * 10 + cellIndex} playerId={playerId} cellModel={cell} 
                onClick={e => {this.cellOnClick(e, colIndex, cellIndex)}} />)}
            </div>)}
        </div>
      </div>);
  }

  cellOnClick = async (event: React.MouseEvent<HTMLButtonElement>, x: number, y: number) => {
    const session = new GameSession(this.props.match.params.sessionId);
    const turnResult = await session.doAction(this.props.match.params.playerId, x, y);
    console.log(turnResult);
  }
}

export interface CellProps {
  playerId: string;
  cellModel: Cell;
  onClick?: (event: React.MouseEvent<HTMLButtonElement>) => void;
}

export class CellComponent extends Component<CellProps, {}> {
  render() {
    return (
      <div style={{borderWidth: "1px", borderStyle: "solid", borderColor: this.props.cellModel.avaliableFor == this.props.playerId ? "green" : "black"}}>
        <button style={{color: this.props.cellModel.alive ? "black" : "gray"}} onClick={this.props.onClick}>
          {
            this.props.cellModel.playerId == emptyPlayerId ? "*" :
            this.props.playerId == this.props.cellModel.playerId ? 
              "X" : "O"}
        </button>
      </div>)
  }
}