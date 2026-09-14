import java.util.HashMap;

public class Csti extends Expr {
    int nr;

    public Csti(int x) {
        this.nr = x;
    }

    @Override
    public String myToString() {
        return Integer.toString(nr);
    }

    public int eval(HashMap<String, Integer> env) {
        return nr;
    }

    public Expr simplify() {
        return this;
    }
}
