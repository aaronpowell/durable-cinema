import { RouteComponentProps } from "react-router-dom";

export interface CastMember {
  character: string;
  actor: string;
  picture: string;
  id: string;
}

export interface Trailer {
  key: string;
  name: string;
  site: string;
}

export interface MovieDetails {
  title: string;
  poster: string;
  runtime?: number;
  overview: string;
  cast: CastMember[];
  videos: Trailer[];
}

export interface MovieRouteParams extends RouteComponentProps<{ id: string }> {}
