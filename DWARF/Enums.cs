//
// Copyright (c) 2023 Antmicro
//
// This file is licensed under the MIT License.
// Full license text is available in 'licenses/MIT.txt'.
//
namespace DWARF
{
    // These enums are used in DWARF4
    public enum DebugInfoEntryType
    {
        NONE,
        DW_TAG_array_type = 0x01,
        DW_TAG_class_type = 0x02,
        DW_TAG_entry_point = 0x03,
        DW_TAG_enumeration_type = 0x04,
        DW_TAG_formal_parameter = 0x05,
        DW_TAG_imported_declaration = 0x08,
        DW_TAG_label = 0x0a,
        DW_TAG_lexical_block = 0x0b,
        DW_TAG_member = 0x0d,
        DW_TAG_pointer_type = 0x0f,
        DW_TAG_reference_type = 0x10,
        DW_TAG_compile_unit = 0x11,
        DW_TAG_string_type = 0x12,
        DW_TAG_structure_type = 0x13,
        DW_TAG_subroutine_type = 0x15,
        DW_TAG_typedef = 0x16,

        DW_TAG_union_type = 0x17,
        DW_TAG_unspecified_parameters = 0x18,
        DW_TAG_variant = 0x19,
        DW_TAG_common_block = 0x1a,
        DW_TAG_common_inclusion = 0x1b,
        DW_TAG_inheritance = 0x1c,
        DW_TAG_inlined_subroutine = 0x1d,
        DW_TAG_module = 0x1e,
        DW_TAG_ptr_to_member_type = 0x1f,
        DW_TAG_set_type = 0x20,
        DW_TAG_subrange_type = 0x21,
        DW_TAG_with_stmt = 0x22,
        DW_TAG_access_declaration = 0x23,
        DW_TAG_base_type = 0x24,
        DW_TAG_catch_block = 0x25,
        DW_TAG_const_type = 0x26,
        DW_TAG_constant = 0x27,
        DW_TAG_enumerator = 0x28,
        DW_TAG_file_type = 0x29,
        DW_TAG_friend = 0x2a,

        DW_TAG_namelist = 0x2b,
        DW_TAG_namelist_item = 0x2c,
        DW_TAG_packed_type = 0x2d,
        DW_TAG_subprogram = 0x2e,
        DW_TAG_template_type_parameter = 0x2f,
        DW_TAG_template_value_parameter = 0x30,
        DW_TAG_thrown_type = 0x31,
        DW_TAG_try_block = 0x32,
        DW_TAG_variant_part = 0x33,
        DW_TAG_variable = 0x34,
        DW_TAG_volatile_type = 0x35,
        DW_TAG_dwarf_procedure = 0x36,
        DW_TAG_restrict_type = 0x37,
        DW_TAG_interface_type = 0x38,
        DW_TAG_namespace = 0x39,
        DW_TAG_imported_module = 0x3a,
        DW_TAG_unspecified_type = 0x3b,
        DW_TAG_partial_unit = 0x3c,
        DW_TAG_imported_unit = 0x3d,
        DW_TAG_condition = 0x3f,

        DW_TAG_shared_type = 0x40,
        DW_TAG_type_unit = 0x41,
        DW_TAG_rvalue_reference_type = 0x42,
        DW_TAG_template_alias = 0x43,

        // DWARF 5
        DW_TAG_coarray_type = 0x44,
        DW_TAG_generic_subrange = 0x45,
        DW_TAG_dynamic_type = 0x46,
        DW_TAG_atomic_type = 0x47,
        DW_TAG_call_site = 0x48,
        DW_TAG_call_site_parameter = 0x49,
        DW_TAG_skeleton_unit = 0x49,
        DW_TAG_immutable_type = 0x4a,

        DW_TAG_lo_user = 0x4080,
        DW_TAG_hi_user = 0xffff,
    }

    public enum AttributeName
    {
        NONE,

        DW_AT_sibling = 0x01, // reference
        DW_AT_location = 0x02, // exprloc, loclistptr
        DW_AT_name = 0x03, // string
        DW_AT_ordering = 0x09, // constant
        DW_AT_byte_size = 0x0b, // constant, exprloc, reference
        DW_AT_bit_offset = 0x0c, // constant, exprloc, reference
        DW_AT_bit_size = 0x0d, // constant, exprloc, reference
        DW_AT_stmt_list = 0x10, // lineptr
        DW_AT_low_pc = 0x11, // address
        DW_AT_high_pc = 0x12, // address, constant
        DW_AT_language = 0x13, // constant
        DW_AT_discr = 0x15, // reference
        DW_AT_discr_value = 0x16, // constant
        DW_AT_visibility = 0x17, // constant
        DW_AT_import = 0x18, // reference
        DW_AT_string_length = 0x19, // exprloc, loclistptr
        DW_AT_common_reference = 0x1a, // reference
        DW_AT_comp_dir = 0x1b, // string
        DW_AT_const_value = 0x1c, // block, constant, string
        DW_AT_containing_type = 0x1d, // reference
        DW_AT_default_value = 0x1e, // reference
        DW_AT_inline = 0x20, // constant
        DW_AT_is_optional = 0x21, // flag
        DW_AT_lower_bound = 0x22, // constant, exprloc, reference
        DW_AT_producer = 0x25, // string
        DW_AT_prototyped = 0x27, // flag
        DW_AT_return_addr = 0x2a, // exprloc, loclistptr
        DW_AT_start_scope = 0x2c, // Constant, rangelistptr
        DW_AT_bit_stride = 0x2e, // constant, exprloc, reference
        DW_AT_upper_bound = 0x2f, // constant, exprloc, reference
        DW_AT_abstract_origin = 0x31, // reference
        DW_AT_accessibility = 0x32, // constant
        DW_AT_address_class = 0x33, // constant
        DW_AT_artificial = 0x34, // flag
        DW_AT_base_types = 0x35, // reference
        DW_AT_calling_convention = 0x36, // constant
        DW_AT_count = 0x37, // constant, exprloc, reference
        DW_AT_data_member_location = 0x38, // constant, exprloc, loclistptr
        DW_AT_decl_column = 0x39, // constant
        DW_AT_decl_file = 0x3a, // constant
        DW_AT_decl_line = 0x3b, // constant
        DW_AT_declaration = 0x3c, // flag
        DW_AT_discr_list = 0x3d, // block
        DW_AT_encoding = 0x3e, // constant
        DW_AT_external = 0x3f, // flag
        DW_AT_frame_base = 0x40, // exprloc, loclistptr
        DW_AT_friend = 0x41, // reference
        DW_AT_identifier_case = 0x42, // constant
        DW_AT_macro_info = 0x43, // macptr
        DW_AT_namelist_item = 0x44, // reference
        DW_AT_priority = 0x45, // reference
        DW_AT_segment = 0x46, // exprloc, loclistptr
        DW_AT_specification = 0x47, // reference
        DW_AT_static_link = 0x48, // exprloc, loclistptr
        DW_AT_type = 0x49, // reference
        DW_AT_use_location = 0x4a, // exprloc, loclistptr
        DW_AT_variable_parameter = 0x4b, // flag
        DW_AT_virtuality = 0x4c, // constant
        DW_AT_vtable_elem_location = 0x4d, // exprloc, loclistptr
        DW_AT_allocated = 0x4e, // constant, exprloc, reference
        DW_AT_associated = 0x4f, // constant, exprloc, reference
        DW_AT_data_location = 0x50, // exprloc
        DW_AT_byte_stride = 0x51, // constant, exprloc, reference
        DW_AT_entry_pc = 0x52, // address
        DW_AT_use_UTF8 = 0x53, // flag
        DW_AT_extension = 0x54, // reference
        DW_AT_ranges = 0x55, // rangelistptr
        DW_AT_trampoline = 0x56, // address, flag, reference, string
        DW_AT_call_column = 0x57, // constant
        DW_AT_call_file = 0x58, // constant
        DW_AT_call_line = 0x59, // constant
        DW_AT_description = 0x5a, // string
        DW_AT_binary_scale = 0x5b, // constant
        DW_AT_decimal_scale = 0x5c, // constant
        DW_AT_small = 0x5d, // reference
        DW_AT_decimal_sign = 0x5e, // constant
        DW_AT_digit_count = 0x5f, // constant
        DW_AT_picture_string = 0x60, // string
        DW_AT_mutable = 0x61, // flag
        DW_AT_threads_scaled = 0x62, // flag
        DW_AT_explicit = 0x63, // flag
        DW_AT_object_pointer = 0x64, // reference
        DW_AT_endianity = 0x65, // constant
        DW_AT_elemental = 0x66, // flag
        DW_AT_pure = 0x67, // flag
        DW_AT_recursive = 0x68, // flag
        DW_AT_signature = 0x69, // reference
        DW_AT_main_subprogram = 0x6a, // flag
        DW_AT_data_bit_offset = 0x6b, // constant
        DW_AT_const_expr = 0x6c, // flag
        DW_AT_enum_class = 0x6d, // flag
        DW_AT_linkage_name = 0x6e, // string

        // DWARF 5
        DW_AT_string_length_bit_size = 0x6f,
        DW_AT_string_length_byte_size = 0x70,
        DW_AT_rank = 0x71,
        DW_AT_str_offsets_base = 0x72,
        DW_AT_addr_base = 0x73,
        DW_AT_rnglists_base = 0x74,
        DW_AT_dwo_name = 0x76,
        DW_AT_reference = 0x77,
        DW_AT_rvalue_reference = 0x78,
        DW_AT_macros = 0x79,
        DW_AT_call_all_calls = 0x7a,
        DW_AT_call_all_source_calls = 0x7b,
        DW_AT_call_all_tail_calls = 0x7c,
        DW_AT_call_return_pc = 0x7d,
        DW_AT_call_value = 0x7e,
        DW_AT_call_origin = 0x7f,
        DW_AT_call_parameter = 0x80,
        DW_AT_call_pc = 0x81,
        DW_AT_call_tail_call = 0x82,
        DW_AT_call_target = 0x83,
        DW_AT_call_target_clobbered = 0x84,
        DW_AT_call_data_location = 0x85,
        DW_AT_call_data_value = 0x86,
        DW_AT_noreturn = 0x87,
        DW_AT_alignment = 0x88,
        DW_AT_export_symbols  = 0x89,
        DW_AT_deleted = 0x89,
        DW_AT_defaulted = 0x8a,
        DW_AT_loclists_base = 0x8b,

        DW_AT_lo_user = 0x20, // 00
        DW_AT_hi_user = 0x3f, // ff

        // Random compiler (gcc) extensions
        DW_AT_MIPS_fde = 0x2001, // 	subprogram tag attribute, offset into .debug_frame section, mips_extensions 	Unknown 	Constant only
        DW_AT_MIPS_loop_begin = 0x2002, // 	Never implemented, mips_extensions 	Nothing 	Constant only
        DW_AT_MIPS_tail_loop_begin = 0x2003, // 	Never implemented, mips_extensions 	Nothing 	Constant only
        DW_AT_MIPS_epilog_begin = 0x2004, // 	Never implemented, mips_extensions 	Nothing 	Constant only
        DW_AT_MIPS_loop_unroll_factor = 0x2005, // 	Never implemented, mips_extensions 	Nothing 	Constant only
        DW_AT_MIPS_software_pipeline_depth = 0x2006, // 	Never implemented, mips_extensions 	Nothing 	Constant only
        DW_AT_MIPS_linkage_name = 0x2007, // 	Same as DWARF4 DW_AT_linkage_name 	GCC 	Constant only
        DW_AT_MIPS_stride = 0x2008, // 	F90 array stride, mips_extensions 	Unknown 	Constant only
        DW_AT_MIPS_abstract_name = 0x2009, // 	name of inlined_subroutine with abstract root in other CU, mips_extensions 	Unknown 	Constant only
        DW_AT_MIPS_clone_origin = 0x200a, // 	name of non-specialed version of cloned subroutine, mips_extensions 	Unknown 	Constant only
        DW_AT_MIPS_has_inlines = 0x200b, // 	hint for inlined subroutines under subprogram DIE, mips_extensions 	Unknown 	Constant only
        DW_AT_MIPS_stride_byte = 0x200c, // 	F90 array stride, mips_extensions 	Unknown 	Constant only
        DW_AT_MIPS_stride_elem = 0x200d, // 	F90 array stride, mips_extensions 	Unknown 	Constant only
        DW_AT_MIPS_ptr_dopetype = 0x200e, // 	F90 Dope Vector, mips_extensions 	Unknown 	Constant only
        DW_AT_MIPS_allocatable_dopetype = 0x200f, // 	F90 Dope Vector, mips_extensions 	Unknown 	Constant only
        DW_AT_MIPS_assumed_shape_dopetype = 0x2010, // 	F90 Dope Vector, mips_extensions 	Unknown 	Constant only
        DW_AT_MIPS_assumed_size = 0x2011, // 	F90 arrays, mips_extensions 	Unknown 	Constant only
        DW_AT_sf_names = 0x2101, // 	DWARF1 only? 	Unknown 	Constant only
        DW_AT_src_info = 0x2102, // 	DWARF1 only? 	Unknown 	Constant only
        DW_AT_mac_info = 0x2103, // 	DWARF1 only? 	Unknown 	Constant only
        DW_AT_src_coords = 0x2104, // 	DWARF1 only? 	Unknown 	Constant only
        DW_AT_body_begin = 0x2105, // 	DWARF1 only? 	Unknown 	Constant only
        DW_AT_body_end = 0x2106, // 	DWARF1 only? 	Unknown 	Constant only
        DW_AT_GNU_vector = 0x2107, // 	ppc/ppc64 Altivec return value 	GCC 	dwfl_module_return_value_location
        DW_AT_GNU_guarded_by = 0x2108, // 	GNU ThreadSafetyAnnotations 	Not implemented 	Constant only
        DW_AT_GNU_pt_guarded_by = 0x2109, // 	GNU ThreadSafetyAnnotations 	Not implemented 	Constant only
        DW_AT_GNU_guarded = 0x210a, // 	GNU ThreadSafetyAnnotations 	Not implemented 	Constant only
        DW_AT_GNU_pt_guarded = 0x210b, // 	GNU ThreadSafetyAnnotations 	Not implemented 	Constant only
        DW_AT_GNU_locks_excluded = 0x210c, // 	GNU ThreadSafetyAnnotations 	Not implemented 	Constant only
        DW_AT_GNU_exclusive_locks_required = 0x210d, // 	GNU ThreadSafetyAnnotations 	Not implemented 	Constant only
        DW_AT_GNU_shared_locks_required = 0x210e, // 	GNU ThreadSafetyAnnotations 	Not implemented 	Constant only
        DW_AT_GNU_odr_signature = 0x210f, // 	link-time ODR checking part of GNU DwarfSeparateTypeInfo 	GCC 	Constant only
        DW_AT_GNU_template_name = 0x2110, // 	GNU Template Parms and DWARF5 proposal 	G++ 	Constant only
        DW_AT_GNU_call_site_value = 0x2111, // 	GNU call site DWARF5 proposal 	GCC 	Recognized in readelf
        DW_AT_GNU_call_site_data_value = 0x2112, // 	GNU call site DWARF5 proposal 	GCC 	Recognized in readelf
        DW_AT_GNU_call_site_target = 0x2113, // 	GNU call site DWARF5 proposal 	GCC 	Recognized in readelf
        DW_AT_GNU_call_site_target_clobbered = 0x2114, // 	GNU call site DWARF5 proposal 	GCC 	Recognized in readelf
        DW_AT_GNU_tail_call = 0x2115, // 	GNU call site DWARF5 proposal 	GCC 	Recognized in readelf
        DW_AT_GNU_all_tail_call_sites = 0x2116, // 	GNU call site DWARF5 proposal 	GCC 	Recognized in readelf
        DW_AT_GNU_all_call_sites = 0x2117, // 	GNU call site DWARF5 proposal 	GCC 	Recognized in readelf
        DW_AT_GNU_all_source_call_sites = 0x2118, // 	GNU call site DWARF5 proposal 	GCC 	Recognized in readelf
        DW_AT_GNU_macros = 0x2119, // 	GNU .debug_macro DWARF5 proposal 	GCC 	Recognized in readelf
        DW_AT_GNU_deleted = 0x211a, // 	Attribute added for C++11 deleted special member functions (= delete;) 	G++ 	Constant only
        DW_AT_GNU_dwo_name = 0x2130, // 	GNU Fission DWARF5 proposal 	Unknown 	No support
        DW_AT_GNU_dwo_id = 0x2131, // 	GNU Fission DWARF5 proposal 	Unknown 	No support
        DW_AT_GNU_ranges_base = 0x2132, // 	GNU Fission DWARF5 proposal 	Unknown 	No support
        DW_AT_GNU_addr_base = 0x2133, // 	GNU Fission DWARF5 proposal 	Unknown 	No support
        DW_AT_GNU_pubnames = 0x2134, // 	GNU Fission DWARF5 proposal 	GCC 	No support
        DW_AT_GNU_pubtypes = 0x2135, // 	GNU Fission DWARF5 proposal 	GCC 	No support
    }

    public enum AttributeType
    {
        DW_FORM_addr = 0x01, // address = address size
        // 0x02 reserved
        DW_FORM_block2 = 0x03, // block =  2 + <>
        DW_FORM_block4 = 0x04, // block = 4 + <>
        DW_FORM_data2 = 0x05, // constant = 2
        DW_FORM_data4 = 0x06, // constant = 4
        DW_FORM_data8 = 0x07, // constant = 8
        DW_FORM_string = 0x08, // string = null terminated string
        DW_FORM_block = 0x09, // block = uleb + <>
        DW_FORM_block1 = 0x0a, // block = 1 + <1..256>
        DW_FORM_data1 = 0x0b, // constant = 1
        DW_FORM_flag = 0x0c, // flag = 1
        DW_FORM_sdata = 0x0d, // constant = sleb
        DW_FORM_strp = 0x0e, // string = address size
        DW_FORM_udata = 0x0f, // constan = uleb
        DW_FORM_ref_addr = 0x10, // reference = address size
        DW_FORM_ref1 = 0x11, // reference = 1
        DW_FORM_ref2 = 0x12, // reference = 2
        DW_FORM_ref4 = 0x13, // reference = 4
        DW_FORM_ref8 = 0x14, // reference = 8
        DW_FORM_ref_udata = 0x15, // reference = uleb
        DW_FORM_indirect = 0x16, // (see Section 7.5.3)
        DW_FORM_sec_offset = 0x17, // lineptr, loclistptr, macptr, rangelistptr = address size
        DW_FORM_exprloc = 0x18, // exprloc = uleb + <DWARF Expression here>
        DW_FORM_flag_present = 0x19, // flag = 0

        // DWARF5
        DW_FORM_strx = 0x1a, // string = uleb
        DW_FORM_addrx = 0x1b, // address = uleb
        DW_FORM_ref_sup4 = 0x1c, // reference = 4
        DW_FORM_strp_sup = 0x1d, // string = address size
        DW_FORM_data16 = 0x1e, // constant = 16
        DW_FORM_line_strp = 0x1f, // string = address size

        // DWARF4
        DW_FORM_ref_sig8 = 0x20, // reference = 8

        // DWARF5
        DW_FORM_implicit_const = 0x21, // constant = 0
        DW_FORM_loclistx = 0x22, // loclist = uleb
        DW_FORM_rnglistx = 0x23, // rnglist = uleb
        DW_FORM_ref_sup8 = 0x24, // reference = 8
        DW_FORM_strx1 = 0x25, // strin = 1
        DW_FORM_strx2 = 0x26, // string = 2
        DW_FORM_strx3 = 0x27, // string = 3
        DW_FORM_strx4 = 0x28, // string = 4
        DW_FORM_addrx1 = 0x29, // address = 1
        DW_FORM_addrx2 = 0x2a, // address = 2
        DW_FORM_addrx3 = 0x2b, // address = 3
        DW_FORM_addrx4 = 0x2c, // address = 4

        // Extensions
        DW_FORM_GNU_addr_index = 0x1f01, // 	GNU Fission DWARF5 proposal 	Unknown 	No support
        DW_FORM_GNU_str_index = 0x1f02, // 	GNU Fission DWARF5 proposal 	Unknown 	No support
        DW_FORM_GNU_ref_alt = 0x1f20, // 	DWZ multifile DWARF5 proposal 	DWZ 	libdw if configured with --enable-dwz
        DW_FORM_GNU_strp_alt = 0x1f21, // 	DWZ multifile DWARF5 proposal 	DWZ 	libdw if configured with --enable-dwz
    }

    public enum StandardLineOpcodes
    {
        EXTENDED = 0,
        DW_LNS_copy = 1,
        DW_LNS_advance_pc = 2,
        DW_LNS_advance_line = 3,
        DW_LNS_set_file = 4,
        DW_LNS_set_column = 5,
        DW_LNS_negate_stmt = 6,
        DW_LNS_set_basic_block = 7,
        DW_LNS_const_add_pc = 8,
        DW_LNS_fixed_advance_pc = 9,
        DW_LNS_set_prologue_end = 10,
        DW_LNS_set_epilogue_begin = 11,
        DW_LNS_set_isa = 12,
    }

    public enum ExtendedLineOpcodes
    {
        DW_LNE_end_sequence = 1,
        DW_LNE_set_address = 2,
        DW_LNE_define_file = 3,
        DW_LNE_set_discriminator = 4, // version 4
    }

    public enum FramestackOpcodes
    {
        DW_CFA_advance_loc = 0b01_000000,
        DW_CFA_offset = 0b10_000000,
        DW_CFA_restore = 0b11_000000,
        DW_CFA_nop = 0b00_000000,
        DW_CFA_set_loc = 1, // 00x01address
        DW_CFA_advance_loc1 = 2, // 00x021-byte delta
        DW_CFA_advance_loc2 = 3, // 00x032-byte delta
        DW_CFA_advance_loc4 = 4, // 00x044-byte delta
        DW_CFA_offset_extended = 5, // 00x05ULEB128 register ULEB128 offset
        DW_CFA_restore_extended = 6, // 00x06ULEB128 register
        DW_CFA_undefined = 7, // 00x07ULEB128 register
        DW_CFA_same_value = 8, // 00x08ULEB128 register
        DW_CFA_register = 9, // 00x09ULEB128 register ULEB128 register
        DW_CFA_remember_state = 10, // 00x0a
        DW_CFA_restore_state = 11, //00x0b
        DW_CFA_def_cfa = 12, // 00x0cULEB128 register ULEB128 offset
        DW_CFA_def_cfa_register = 13, // 00x0dULEB128 register
        DW_CFA_def_cfa_offset = 14, // 00x0eULEB128 offset
        DW_CFA_def_cfa_expression = 15, // 00x0fBLOCK
        DW_CFA_expression = 16,

        DW_CFA_offset_extended_sf = 17, // ‡00x11ULEB128 register SLEB128 offset
        DW_CFA_def_cfa_sf = 18, // ‡00x12ULEB128 register SLEB128 offset
        DW_CFA_def_cfa_offset_sf = 19, // ‡00x13SLEB128 offset
        DW_CFA_val_offset = 20, // ‡00x14ULEB128 ULEB128
        DW_CFA_val_offset_sf = 21, // ‡00x15ULEB128 SLEB128
        DW_CFA_val_expression = 0x16, // ‡00x16ULEB128 BLOCK
        DW_CFA_lo_user = 0x1c, // 00x1c
        DW_CFA_hi_user = 0x3f, // 00x3f Operand 1 Operand 2
    }

    public enum RangeEntryType
    {
        DW_RLE_end_of_list = 1,
        DW_RLE_base_addressx,
        DW_RLE_startx_endx,
        DW_RLE_startx_length,
        DW_RLE_offset_pair,

        // Following may be used only in non split units:
        DW_RLE_base_address,
        DW_RLE_start_end,
        DW_RLE_start_length,
    }

    public enum LocationEntryType
    {
       DW_LLE_end_of_list = 1,
       DW_LLE_base_addressx,
       DW_LLE_startx_endx,
       DW_LLE_startx_length,
       DW_LLE_offset_pair,
       DW_LLE_defualt_location,

       // Following may be used only in non split units:
       DW_LLE_base_address,
       DW_LLE_start_end,
       DW_LLE_start_length,
    }

    public enum StandardContentDescription
    {
        DW_LNCT_path = 1,
        DW_LNCT_directory_index,
        DW_LNCT_timestamp,
        DW_LNCT_size,
        DW_LNCT_MD5,
    }
}
