import React from "react";
import {
  Container,
  Grid,
  Typography,
  GridListTileBar,
  GridListTile,
  GridList,
  ListSubheader,
  Button,
  CircularProgress
} from "@material-ui/core";
import { makeStyles, createStyles, Theme } from "@material-ui/core/styles";
import useMovie from "../hooks/useMovie";
import { Link } from "react-router-dom";
import { MovieRouteParams } from "../typings";

const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    button: {
      margin: theme.spacing(1)
    },
    input: {
      display: "none"
    },
    link: {
      textDecoration: "none"
    },
    root: {
      padding: theme.spacing(1)
    }
  })
);

const MovieDetailsComponent: React.FC<MovieRouteParams> = ({ match }) => {
  let id = match.params.id;
  const classes = useStyles();

  const [movie, isLoading] = useMovie(id);

  if (isLoading || movie === undefined) {
    return (
      <Container maxWidth="xl" className={classes.root}>
        <CircularProgress />
      </Container>
    );
  }

  return (
    <Container className={classes.root} maxWidth="xl">
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
          <Link to={`/sessions/${id}`} className={classes.link}>
            <Button
              variant="contained"
              color="primary"
              className={classes.button}
            >
              View Sessions
            </Button>
          </Link>
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

export default MovieDetailsComponent;
