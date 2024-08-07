import React from 'react';
import { useField, useFormikContext } from 'formik';
import { Select, MenuItem, FormControl, InputLabel, FormHelperText } from '@mui/material';

const FormikSelect = ({ name, label, options, ...props }) => {
  const { setFieldValue } = useFormikContext();
  const [field, meta] = useField(name);

  const handleChange = (event) => {
    setFieldValue(name, event.target.value);
  };

  return (
    <FormControl variant="filled" fullWidth error={meta.touched && Boolean(meta.error)}>
      <InputLabel>{label}</InputLabel>
      <Select
        {...field}
        {...props}
        value={field.value || ''}
        onChange={handleChange}
      >
        {options.map((option) => (
          <MenuItem key={option.value} value={option.value}>
            {option.label}
          </MenuItem>
        ))}
      </Select>
      {meta.touched && meta.error ? (
        <FormHelperText>{meta.error}</FormHelperText>
      ) : null}
    </FormControl>
  );
};

export default FormikSelect;
