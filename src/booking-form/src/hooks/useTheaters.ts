import { useState, useEffect } from "react";

interface Theater {
  id: number;
  name: string;
  capacity: number;
}

const useSessions = (movieId: string) => {
  const [theaters, setTheaters] = useState<Theater[]>();
  const [isLoading, setLoading] = useState(true);

  useEffect(() => {
    fetch("http://localhost:7071/api/theaters/" + movieId)
      .then(res => res.json())
      .then((theaters: Theater[]) => {
        setTheaters(theaters);
        setLoading(false);
      });
  }, [movieId]);

  return [theaters, isLoading];
};

export default useSessions;
