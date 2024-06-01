import { isRef } from 'vue';

// generate a function to set the content of a  Quill v2 editor on load
// you can pass in either a value or a ref
export const generateQuillInitializer = (value) => {
  return ({ instance }) => {
    instance.setContents(
      instance.clipboard.convert({
        html: isRef(value) ? value.value : value,
      }),
    );
  };
};
