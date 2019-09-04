import React, { useState, useEffect } from "react";
import Typography from "@material-ui/core/Typography";
import Container from "@material-ui/core/Container";
import CircularProgress from "@material-ui/core/CircularProgress";
import { GridList, GridListTile } from "@material-ui/core";
import { Link } from "react-router-dom";

interface Movie {
  id: number;
  overview: string;
  title: string;
  poster: string;
}

const MovieCell: React.FC<{ movie: Movie }> = ({ movie }) => {
  return (
    <GridListTile
      style={{ height: 140, width: 100, margin: 5 }}
      title={movie.title}
      onClick={() => console.log("click")}
    >
      <Link to={`/movie/${movie.id}`}>
        <img src={movie.poster} alt={movie.title} />
      </Link>
    </GridListTile>
  );
};

const NowShowing: React.FC = () => {
  let [isLoading, setIsLoading] = useState(true);
  let [movies, setMovies] = useState<Movie[]>([]);

  useEffect(() => {
    fetch("http://localhost:7071/api/movies")
      .then(res => res.json())
      .then((movies: Movie[]) => {
        setMovies(movies);
        setIsLoading(false);
      });
  }, []);

  return (
    <Container maxWidth="lg">
      <Typography
        component="h1"
        variant="h2"
        align="center"
        color="textPrimary"
        gutterBottom
      >
        Now Showing
      </Typography>
      <Typography variant="h5" align="center" color="textSecondary" paragraph>
        Check out the movies that are currently playing at the Durable Cinema.
      </Typography>
      {isLoading && <CircularProgress />}
      {movies.length !== 0 && (
        <GridList spacing={2}>
          {movies.map(m => (
            <MovieCell movie={m} key={m.id} />
          ))}
        </GridList>
      )}
    </Container>
  );
};

export default NowShowing;
