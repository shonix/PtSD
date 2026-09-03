import java.util.HashMap;

public abstract class Expr {
    @Override
    public abstract String toString();
    public abstract int eval(HashMap<String, Integer> env);
    public abstract Expr simplify();
}
