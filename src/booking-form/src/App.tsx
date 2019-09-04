import React from "react";
import NowShowing from "./components/NowShowing";
import AppBar from "@material-ui/core/AppBar";
import Toolbar from "@material-ui/core/Toolbar";
import Typography from "@material-ui/core/Typography";
import { Theme, createStyles, makeStyles } from "@material-ui/core/styles";
import { BrowserRouter as Router, Route, Link } from "react-router-dom";
import MovieDetails from "./components/MovieDetails";
import SvgIcon, { SvgIconProps } from "@material-ui/core/SvgIcon";

const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    headerLink: {
      color: "#fff",
      textDecoration: "none"
    }
  })
);

function HomeIcon(props: SvgIconProps) {
  return (
    <SvgIcon {...props}>
      <path d="M10 20v-6h4v6h5v-8h3L12 3 2 12h3v8z" />
    </SvgIcon>
  );
}

const App: React.FC = () => {
  let styles = useStyles();
  return (
    <Router>
      <AppBar position="relative">
        <Toolbar>
          <Link to="/" className={styles.headerLink}>
            <Typography variant="h6" color="inherit" noWrap>
              <HomeIcon />
              Durable Cinema
            </Typography>
          </Link>
        </Toolbar>
      </AppBar>
      <main>
        <Route path="/" exact component={NowShowing} />
        <Route path="/movie/:id" component={MovieDetails} />
      </main>
    </Router>
  );
};

export default App;
