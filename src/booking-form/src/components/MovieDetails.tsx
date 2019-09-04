import React, { useState, useEffect } from "react";
import { RouteComponentProps } from "react-router-dom";
import {
  Container,
  Grid,
  Typography,
  GridListTileBar,
  GridListTile,
  GridList,
  ListSubheader
} from "@material-ui/core";

interface CastMember {
  character: string;
  actor: string;
  picture: string;
  id: string;
}

interface Trailer {
  key: string;
  name: string;
  site: string;
}

interface MovieDetails {
  title: string;
  poster: string;
  runtime?: number;
  overview: string;
  cast: CastMember[];
  videos: Trailer[];
}

const MovieDetails: React.FC<RouteComponentProps<{ id: string }>> = ({
  match
}) => {
  let id = match.params.id;

  let [movie, setMovie] = useState<MovieDetails>();

  useEffect(() => {
    fetch("http://localhost:7071/api/movies/" + id)
      .then(res => res.json())
      .then(movie => setMovie(movie));
  }, [id]);

  if (!movie) {
    return null;
  }

  return (
    <Container style={{ padding: "10px" }} maxWidth="xl">
      <Grid container spacing={3}>
        <Grid item xs={4}>
          <img src={movie.poster} alt={movie.title} height={800} />
        </Grid>
        <Grid item xs={8}>
          <Typography
            component="h1"
            variant="h2"
            align="left"
            color="textPrimary"
            gutterBottom
          >
            {movie.title}
          </Typography>
          <Typography variant="body1" gutterBottom>
            {movie.overview}
          </Typography>
          <Typography variant="caption" display="block" gutterBottom>
            Runtime: {movie.runtime ? `${movie.runtime} minutes` : "TBC"}
          </Typography>
          <GridList cellHeight={200} cols={4}>
            <GridListTile key="Subheader" cols={4} style={{ height: "auto" }}>
              <ListSubheader component="div">Cast</ListSubheader>
            </GridListTile>
            {movie.cast.map(cast => (
              <GridListTile key={cast.id}>
                <img src={cast.picture} alt={cast.actor} />
                <GridListTileBar
                  title={cast.actor}
                  subtitle={<span>{cast.character}</span>}
                />
              </GridListTile>
            ))}
          </GridList>
        </Grid>

        {movie.videos
          .filter(v => v.site === "YouTube")
          .map(v => (
            <Grid item xs>
              <iframe
                src={`https://www.youtube.com/embed/${v.key}`}
                id={v.key}
                width="640"
                height="360"
                frameBorder={0}
                title={v.name}
              ></iframe>
            </Grid>
          ))}
      </Grid>
    </Container>
  );
};

export default MovieDetails;
