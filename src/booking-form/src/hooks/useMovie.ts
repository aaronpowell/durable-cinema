import { useState, useEffect } from "react";
import { MovieDetails } from "../typings";

const useMovie = (id: string): [MovieDetails | undefined, boolean] => {
  let [movie, setMovie] = useState<MovieDetails>();
  let [isLoading, setLoading] = useState(true);

  useEffect(() => {
    fetch("http://localhost:7071/api/movies/" + id)
      .then(res => res.json())
      .then(movie => {
        setMovie(movie);
        setLoading(false);
      });
  }, [id]);

  return [movie, isLoading];
};

export default useMovie;
