import { dirname } from "path";
import { fileURLToPath } from "url";
import { FlatCompat } from "@eslint/eslintrc";

const __filename = fileURLToPath(import.meta.url);
const __dirname = dirname(__filename);

const RuleStatus = {
  Off: "off",
  Warn: "warn",
  Error: "error",
};

const compat = new FlatCompat({
  baseDirectory: __dirname,
});

const eslintConfig = [
  ...compat.config({
    extends: ["next/core-web-vitals", "next/typescript", "prettier"],
    rules: {
      "@typescript-eslint/no-unused-vars": RuleStatus.Warn,
      "@typescript-eslint/no-require-imports": RuleStatus.Warn,
    },
  }),
];

export default eslintConfig;
